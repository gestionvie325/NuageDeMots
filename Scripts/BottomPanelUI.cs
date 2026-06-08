using UnityEngine;
using TMPro;





/*public class BottomPanelUI : MonoBehaviour
{
    public RectTransform panel;

    public float hiddenY = -350f;

    public float shownY = 0f;

    public float speed = 5f;

    private bool isOpen = false;

    public TMP_Text definitionText;


*/










public class BottomPanelUI : MonoBehaviour
{

    // comportement du panneau menu
    public RectTransform panel;

    public TMP_Text definitionText;

    public float hiddenY = -300f;

    public float shownY = 0f;

    public float speed = 5f;

    private bool isOpen = false;

    public static BottomPanelUI instance;



    void Update()
    {
        Vector2 pos =
            panel.anchoredPosition;

        float targetY =
            isOpen ? shownY : hiddenY;

        pos.y = Mathf.Lerp(
            pos.y,
            targetY,
            Time.deltaTime * speed
        );

        panel.anchoredPosition = pos;
    }

    public void TogglePanel()
    {
        isOpen = !isOpen;
    }

    void Awake()
    {
        instance = this;
    }
// On met la definition dans le texte pour definition du panneau
    public void ShowDefinition(string def)
    {
        Debug.Log("Définition reçue par le panneau : " + def);

        if (definitionText == null)
        {
            Debug.LogError("definitionText n'est pas assigné !");
            return;
        }

        definitionText.text = def;

        isOpen = true;
    }
}













    /*

    public void ShowDefinition(string def)
{
    definitionText.text = def;

    isOpen = true;
}


    void Update()
    {
        Vector2 pos = panel.anchoredPosition;

        float targetY = isOpen ? shownY : hiddenY;

        pos.y = Mathf.Lerp(
            pos.y,
            targetY,
            Time.deltaTime * speed
        );

        panel.anchoredPosition = pos;
    }

    public void TogglePanel()
    {
        isOpen = !isOpen;
    }
}
*/
