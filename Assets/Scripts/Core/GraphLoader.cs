using UnityEngine;

public class GraphLoader : MonoBehaviour
{
    public GraphData graphData;

    void Awake()
    {
        LoadGraph();
    }

    void LoadGraph()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("graph");

        if (jsonFile == null)
        {
            Debug.LogError("Τΰιλ graph.json νε νΰιδεν β Resources!");
            return;
        }

        graphData = JsonUtility.FromJson<GraphData>(jsonFile.text);
    }
}
