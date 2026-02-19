using UnityEngine;
using System.Collections.Generic;

public class GraphManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject edgePrefab;
    public float radius = 4f;
    public float verticalLimit = 2f;

    private Dictionary<string, GameObject> spawnedNodes =
        new Dictionary<string, GameObject>();

    private List<EdgeBehaviour> spawnedEdges =
        new List<EdgeBehaviour>();

    private Dictionary<string, List<string>> adjacency =
        new Dictionary<string, List<string>>();

    private Dictionary<string, Color> categoryColors =
        new Dictionary<string, Color>();

    private GraphLoader loader;

    void Start()
    {
        loader = GetComponent<GraphLoader>();

        GenerateCategoryColors();
        SpawnNodes();
        SpawnEdges();
    }

    void GenerateCategoryColors()
    {
        HashSet<string> categories = new HashSet<string>();

        foreach (var node in loader.graphData.nodes)
            categories.Add(node.category);

        int index = 0;

        foreach (var category in categories)
        {
            float hue = (float)index / categories.Count;
            Color color = Color.HSVToRGB(hue, 0.6f, 0.9f);

            categoryColors.Add(category, color);
            index++;
        }
    }

    void SpawnNodes()
    {
        int count = loader.graphData.nodes.Count;

        for (int i = 0; i < count; i++)
        {
            NodeData node = loader.graphData.nodes[i];

            Vector3 position = GetPointOnSphere(i, count) * radius;

            position.y = Mathf.Clamp(position.y, -verticalLimit, verticalLimit);

            GameObject nodeObj =
                Instantiate(nodePrefab, position, Quaternion.identity);

            nodeObj.transform.localScale = Vector3.one * 0.5f;

            NodeBehaviour behaviour =
                nodeObj.GetComponent<NodeBehaviour>();

            behaviour.Initialize(node);

            if (categoryColors.ContainsKey(node.category))
                behaviour.SetBaseColor(categoryColors[node.category]);

            spawnedNodes.Add(node.id, nodeObj);
        }
    }


    void SpawnEdges()
    {
        foreach (var link in loader.graphData.links)
        {
            if (!spawnedNodes.ContainsKey(link.source) ||
                !spawnedNodes.ContainsKey(link.target))
                continue;

            GameObject edgeObj = Instantiate(edgePrefab);

            EdgeBehaviour edge =
                edgeObj.GetComponent<EdgeBehaviour>();

            edge.startNode = spawnedNodes[link.source].transform;
            edge.endNode = spawnedNodes[link.target].transform;

            edge.sourceId = link.source;
            edge.targetId = link.target;

            spawnedEdges.Add(edge);

            RegisterAdjacency(link.source, link.target);
        }
    }

    void RegisterAdjacency(string a, string b)
    {
        if (!adjacency.ContainsKey(a))
            adjacency[a] = new List<string>();

        if (!adjacency.ContainsKey(b))
            adjacency[b] = new List<string>();

        adjacency[a].Add(b);
        adjacency[b].Add(a);
    }

    public void HighlightNode(string nodeId)
    {
        ResetHighlight();

        if (!spawnedNodes.ContainsKey(nodeId))
            return;

        spawnedNodes[nodeId]
            .GetComponent<NodeBehaviour>()
            .Highlight(true);

        if (adjacency.ContainsKey(nodeId))
        {
            foreach (var neighborId in adjacency[nodeId])
            {
                spawnedNodes[neighborId]
                    .GetComponent<NodeBehaviour>()
                    .Highlight(true);
            }
        }

        foreach (var edge in spawnedEdges)
        {
            if (edge.sourceId == nodeId ||
                edge.targetId == nodeId)
                edge.Highlight(true);
        }
    }

    public void ResetHighlight()
    {
        foreach (var node in spawnedNodes.Values)
            node.GetComponent<NodeBehaviour>().Highlight(false);

        foreach (var edge in spawnedEdges)
            edge.Highlight(false);
    }

    Vector3 GetPointOnSphere(int index, int total)
    {
        float offset = 2f / total;
        float increment = Mathf.PI * (3f - Mathf.Sqrt(5f));

        float y = ((index * offset) - 1) + (offset / 2);
        float r = Mathf.Sqrt(1 - y * y);

        float phi = index * increment;

        float x = Mathf.Cos(phi) * r;
        float z = Mathf.Sin(phi) * r;

        return new Vector3(x, y, z);
    }
}
