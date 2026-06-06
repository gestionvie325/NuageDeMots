using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Xml;
using System.IO;

[System.Serializable]
public class WordData
{
    public string mot;
    public string definition;
}

public class WordCloudManager : MonoBehaviour
{
    public GameObject wordPrefab;
    public int numberOfWords = 100;

    public float spreadX = 25f;
    public float spreadY = 12f;
    public float spacingZ = 15f;

    public List<WordData> words = new List<WordData>();



    //******
    public float recycleDistanceBehindCamera = 20f;
    public float recycleDistanceAhead = 500f;

    private int nextWordIndex = 0;
    private Camera cam;
    //*******//




    void ShuffleWords()
{
    for (int i = 0; i < words.Count; i++)
    {
        int randomIndex = Random.Range(i, words.Count);

        WordData temp = words[i];
        words[i] = words[randomIndex];
        words[randomIndex] = temp;
    }
}





    void Start()
    {

        cam = Camera.main;
        LoadXML();

        ShuffleWords();

        if (words.Count == 0)
        {
            Debug.LogError("Aucun mot chargé depuis le XML.");
            return;
        }

        if (wordPrefab == null)
        {
            Debug.LogError("wordPrefab n'est pas assigné dans l'inspecteur.");
            return;
        }

        GenerateWords();
    }

    void Update()
{
    RecycleWords();
}

    void RecycleWords()
{
    if (cam == null) return;

    foreach (WordBehaviour word in WordBehaviour.allWords)
    {
        if (word.transform.position.z < cam.transform.position.z - recycleDistanceBehindCamera)
        {
            RepositionWord(word);
        }
    }
}


void RepositionWord(WordBehaviour word)
{
    float x = Random.Range(-spreadX, spreadX);
    float y = Random.Range(-spreadY, spreadY);
    float z = cam.transform.position.z + recycleDistanceAhead + Random.Range(0f, 200f);

    word.transform.position = new Vector3(x, y, z);

    WordData wd = GetNextRandomWord();

    word.SetWord(wd.mot, wd.definition);
}

WordData GetNextRandomWord()
{
    if (nextWordIndex >= words.Count)
    {
        ShuffleWords();
        nextWordIndex = 0;
    }

    WordData wd = words[nextWordIndex];
    nextWordIndex++;

    return wd;
}




    void LoadXML()
    {
        /*string path = Path.Combine(Application.streamingAssetsPath, "dictionary.xml");
         *
         Ancien ok*/
        //Nouvelle solution pour windows et android :
        TextAsset xmlFile = Resources.Load<TextAsset>("dictionary");
        XmlDocument doc = new XmlDocument();


        if (xmlFile != null)
        {
            string xmlText = xmlFile.text;

            doc.LoadXml(xmlText);

            XmlNodeList words = doc.GetElementsByTagName("word");

            foreach (XmlNode word in words)
            {
                Debug.Log(word.InnerText);
            }
        }
        else
        {
            Debug.LogError("dictionary.xml introuvable");
        }





/*
        Debug.Log(path);

        if (File.Exists(path))
        {
            string xmlContent = File.ReadAllText(path);
            Debug.Log(xmlContent);
        }
        else
        {
            Debug.LogError("Fichier XML introuvable !");
        }
   */





    /*    string path = Path.Combine(Application.dataPath, "StreamingAssets/dictionary.xml");
*/
    /*    XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);
ancien ok */
/*
    UnityWebRequest request = UnityWebRequest.Get(
    System.IO.Path.Combine(Application.streamingAssetsPath, "dictionary.xml")
);

    doc.LoadXml(request.downloadHandler.text);
   */



        XmlNodeList wordNodes = doc.SelectNodes("//element");

        foreach (XmlNode node in wordNodes)
        {
            WordData wd = new WordData();

            wd.mot = node["mot"].InnerText;
            wd.definition = node["definition"].InnerText;

            words.Add(wd);
        }

        Debug.Log("Nombre de mots chargés : " + words.Count);
    }

    void GenerateWords()
    {
        for (int i = 0; i < numberOfWords; i++)
        {
            float x = Random.Range(-spreadX, spreadX);
            float y = Random.Range(-spreadY, spreadY);
            float z = i * spacingZ;

            Vector3 position = new Vector3(x, y, z);

            GameObject wordObj = Instantiate(
                wordPrefab,
                position,
                Quaternion.identity
            );

            TMP_Text txt = wordObj.GetComponentInChildren<TMP_Text>();

            if (txt == null)
            {
                Debug.LogError("Aucun TMP_Text trouvé dans le prefab.");
                continue;
            }

            WordData wd = GetNextRandomWord();

WordBehaviour wb = wordObj.GetComponent<WordBehaviour>();

if (wb != null)
{
    wb.SetWord(wd.mot, wd.definition);
}

            //WordBehaviour wb = wordObj.GetComponent<WordBehaviour>();

            if (wb != null)
            {
                wb.definition = wd.definition;
            }
            else
            {
                Debug.LogError("Aucun WordBehaviour trouvé sur le prefab.");
            }
        }
    }
}
