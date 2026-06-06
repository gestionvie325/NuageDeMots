using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WordBehaviour : MonoBehaviour
{
    public static List<WordBehaviour> allWords =
        new List<WordBehaviour>();

    public string definition;

    private Camera cam;

    private TMP_Text textMesh;

    void OnEnable()
    {
        if (!allWords.Contains(this))
        {
            allWords.Add(this);
        }
    }

    void OnDisable()
    {
        allWords.Remove(this);
    }

    void Start()
    {
        cam = Camera.main;

        textMesh =
            GetComponentInChildren<TMP_Text>();
    }

    void Update()
    {
        if (cam == null) return;

        transform.LookAt(cam.transform);

        transform.Rotate(0, 180, 0);
    }

    public void SetWord(
        string newWord,
        string newDefinition
    )
    {
        if (textMesh == null)
        {
            textMesh =
                GetComponentInChildren<TMP_Text>();
        }

        if (textMesh != null)
        {
            textMesh.text = newWord;
        }

        definition = newDefinition;
    }
}
