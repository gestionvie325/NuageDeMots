using UnityEngine;

public class WordClickManager : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (cam == null)
            Debug.LogError("Aucune caméra avec le tag MainCamera.");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            DetectClickedWord();
        }
    }

    void DetectClickedWord()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Raycast touche : " + hit.collider.name);

            WordBehaviour word =
                hit.collider.GetComponentInParent<WordBehaviour>();

            if (word == null)
            {
                Debug.LogWarning("L'objet touché n'a pas de WordBehaviour parent.");
                return;
            }

            Debug.Log("Mot cliqué : " + word.name);
            Debug.Log("Définition : " + word.definition);

            if (BottomPanelUI.instance != null)
                BottomPanelUI.instance.ShowDefinition(word.definition);
            else
                Debug.LogError("BottomPanelUI.instance est null.");

            if (CameraFocusController.instance != null)
                CameraFocusController.instance.FocusOnWord(word.transform);
            else
                Debug.LogError("CameraFocusController.instance est null.");
        }
        else
        {
            Debug.Log("Raycast ne touche rien.");
        }
    }
}
