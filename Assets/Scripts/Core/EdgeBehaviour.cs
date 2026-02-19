using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EdgeBehaviour : MonoBehaviour
{
    public Transform startNode;
    public Transform endNode;

    public string sourceId;
    public string targetId;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        if (startNode == null || endNode == null)
            return;

        line.SetPosition(0, startNode.position);
        line.SetPosition(1, endNode.position);
    }

    public void Highlight(bool state)
    {
        if (state)
            line.material.color = Color.yellow;
        else
            line.material.color = Color.white;
    }
}
