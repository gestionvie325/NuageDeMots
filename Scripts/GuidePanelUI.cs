using UnityEngine;
using TMPro;

public class GuidePanelUI : MonoBehaviour
{
    [Header("Panneau du guide")]
    public GameObject guidePanel;

    [Header("Texte du guide")]
    public TMP_Text guideText;

    [TextArea(10, 30)]
    public string guideContent;

    void Awake()
    {
        if (guidePanel != null)
        {
            guidePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("GuidePanelUI : guidePanel n'est pas assigné.");
        }
    }

    void Start()
    {
        UpdateGuideText();
    }

    void UpdateGuideText()
    {
        if (guideText != null)
        {
            guideText.text = guideContent;
        }
        else
        {
            Debug.LogWarning("GuidePanelUI : guideText n'est pas assigné.");
        }
    }

    public void OpenGuide()
    {
        if (guidePanel == null)
        {
            Debug.LogError("GuidePanelUI : impossible d'ouvrir, guidePanel est null.");
            return;
        }

        UpdateGuideText();

        guidePanel.SetActive(true);

        Debug.Log("Guide ouvert");
    }

    public void CloseGuide()
    {
        if (guidePanel == null)
        {
            Debug.LogError("GuidePanelUI : impossible de fermer, guidePanel est null.");
            return;
        }

        guidePanel.SetActive(false);

        Debug.Log("Guide fermé");
    }
}
