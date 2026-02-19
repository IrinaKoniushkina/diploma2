using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject panel;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;

    public Transform playerCamera;

    void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void ShowNode(NodeData data)
    {
        panel.SetActive(true);

        titleText.text = data.title;
        descriptionText.text = BuildText(data);

        PositionPanel();
    }

    public void HidePanel()
    {
        panel.SetActive(false);
    }

    void PositionPanel()
    {
        panel.transform.position =
            playerCamera.position + playerCamera.forward * 2f;

        panel.transform.LookAt(playerCamera);
    }

    string BuildText(NodeData data)
    {
        string text = data.description;

        if (!string.IsNullOrEmpty(data.h2))
            text += "\n\n" + data.h2.ToUpper();

        if (!string.IsNullOrEmpty(data.part2))
            text += "\n\n" + data.part2;

        return text;
    }
}
