using UnityEngine;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit;

public class NodeBehaviour : MonoBehaviour
{
    public NodeData data;
    public TextMeshPro titleText;

    private Renderer rend;
    private Material materialInstance;

    private GraphManager graphManager;

    private Color baseColor = Color.white;

    // сила свечения
    private float glowIntensity = 0.5f;
    private float highlightIntensity = 2.0f;

    void Awake()
    {
        rend = GetComponent<Renderer>();

        // создаём копию материала (чтобы не менять prefab глобально)
        materialInstance = rend.material;
    }

    void Start()
    {
        graphManager = FindObjectOfType<GraphManager>();
    }

    public void Initialize(NodeData nodeData)
    {
        data = nodeData;
        titleText.text = nodeData.title;
    }

    public void SetBaseColor(Color color)
    {
        baseColor = color;

        materialInstance.color = baseColor;

        SetGlow(glowIntensity);
    }

    void SetGlow(float intensity)
    {
        if (materialInstance.HasProperty("_EmissionColor"))
        {
            Color emissionColor = baseColor * intensity;
            materialInstance.SetColor("_EmissionColor", emissionColor);
            materialInstance.EnableKeyword("_EMISSION");
        }
    }

    public void OnHoverEnter()
    {
        graphManager.HighlightNode(data.id);
    }

    public void OnHoverExit()
    {
        graphManager.ResetHighlight();
    }

    public void OnSelect()
    {
        graphManager.HighlightNode(data.id);
        UIManager.Instance.ShowNode(data);
        Debug.Log("Node Selected!");
    }

    public void Highlight(bool state)
    {
        if (state)
        {
            materialInstance.color = Color.white;
            SetGlow(highlightIntensity);
        }
        else
        {
            materialInstance.color = baseColor;
            SetGlow(glowIntensity);
        }
    }
}
