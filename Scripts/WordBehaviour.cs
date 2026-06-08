using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class WordBehaviour : MonoBehaviour
{
    public static List<WordBehaviour> allWords =
        new List<WordBehaviour>();

        public static WordBehaviour selectedWord;

    public string definition;

    private Camera cam;

    private TMP_Text textMesh;



    [Header("Transparence selon la distance")]
    public float transparentDistance = 250f;
    public float opaqueDistance = 40f;

    public float minAlpha = 0.05f;
    public float maxAlpha = 1f;



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



            UpdateTransparency();
    }

    void Update()
    {
        if (cam == null) return;

        transform.LookAt(cam.transform);

        transform.Rotate(0, 180, 0);

        UpdateTransparency();


    }



    void UpdateTransparency()
    {
        if (textMesh == null) return;
        if (cam == null) return;

        float distance =
            Vector3.Distance(
                cam.transform.position,
                transform.position
            );

        float alpha =
            Mathf.InverseLerp(
                transparentDistance,
                opaqueDistance,
                distance
            );

        alpha =
            Mathf.Clamp01(alpha);

        alpha =
            Mathf.Lerp(
                minAlpha,
                maxAlpha,
                alpha
            );

        Color color =
            textMesh.color;

        color.a = alpha;

        textMesh.color = color;


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
            //pour eviter que le mot soit sur 2 lignes :
            textMesh.enableWordWrapping = false;
            textMesh.overflowMode = TextOverflowModes.Overflow;


            textMesh.text = newWord;

            UpdateTransparency();
        }



        definition = newDefinition;
    }



}
