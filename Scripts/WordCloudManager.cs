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
    public static WordCloudManager instance;

    public GameObject wordPrefab;
    public int numberOfWords = 100;

    public float spreadX = 25f;
    public float spreadY = 12f;
    public float spacingZ = 15f;

    public List<WordData> words = new List<WordData>();

    public float recycleDistanceBehindCamera = 20f;
    public float recycleDistanceAhead = 500f;

    private int nextWordIndex = 0;
    private Camera cam;

    private List<GameObject> spawnedWordObjects =
        new List<GameObject>();

    string SavePath
    {
        get
        {
            return Path.Combine(
                Application.persistentDataPath,
                "dictionary.xml"
            );
        }
    }

    void Awake()
    {
        instance = this;
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

    void RecycleWords()
    {
        if (cam == null) return;

        foreach (WordBehaviour word in WordBehaviour.allWords)
        {
            if (word == null) continue;

            if (word.transform.position.z <
                cam.transform.position.z - recycleDistanceBehindCamera)
            {
                RepositionWord(word);
            }
        }
    }

    void RepositionWord(WordBehaviour word)
    {
        float x = Random.Range(-spreadX, spreadX);
        float y = Random.Range(-spreadY, spreadY);

        float z =
            cam.transform.position.z
            + recycleDistanceAhead
            + Random.Range(0f, 200f);

        word.transform.position = new Vector3(x, y, z);

        WordData wd = GetNextRandomWord();

        word.SetWord(wd.mot, wd.definition);

        TextMeshCollider colliderUpdater =
            word.GetComponentInChildren<TextMeshCollider>();

        if (colliderUpdater != null)
        {
            colliderUpdater.UpdateCollider();
        }
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
        words.Clear();

        XmlDocument doc = new XmlDocument();

        if (File.Exists(SavePath))
        {
            string xmlText = File.ReadAllText(SavePath);
            doc.LoadXml(xmlText);

            Debug.Log("Dictionnaire chargé depuis : " + SavePath);
        }
        else
        {
            TextAsset xmlFile =
                Resources.Load<TextAsset>("dictionary");

            if (xmlFile == null)
            {
                Debug.LogError("dictionary.xml introuvable dans Resources.");
                return;
            }

            doc.LoadXml(xmlFile.text);

            Debug.Log("Dictionnaire chargé depuis Resources.");
        }

        XmlNodeList wordNodes =
            doc.SelectNodes("//element");

        foreach (XmlNode node in wordNodes)
        {
            if (node["mot"] == null)
                continue;

            if (node["definition"] == null)
                continue;

            WordData wd = new WordData();

            wd.mot = node["mot"].InnerText;
            wd.definition = node["definition"].InnerText;

            words.Add(wd);
        }

        Debug.Log("Nombre de mots chargés : " + words.Count);
    }

    void GenerateWords()
    {
        ClearGeneratedWords();

        nextWordIndex = 0;

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

            spawnedWordObjects.Add(wordObj);

            TMP_Text txt =
                wordObj.GetComponentInChildren<TMP_Text>();

            if (txt == null)
            {
                Debug.LogError("Aucun TMP_Text trouvé dans le prefab.");
                continue;
            }

            WordData wd = GetNextRandomWord();

            WordBehaviour wb =
                wordObj.GetComponent<WordBehaviour>();

            if (wb != null)
            {
                wb.SetWord(wd.mot, wd.definition);
            }
            else
            {
                Debug.LogError("Aucun WordBehaviour trouvé sur le prefab.");
            }

            TextMeshCollider colliderUpdater =
                wordObj.GetComponentInChildren<TextMeshCollider>();

            if (colliderUpdater != null)
            {
                colliderUpdater.UpdateCollider();
            }
        }
    }

    void ClearGeneratedWords()
    {
        foreach (GameObject obj in spawnedWordObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }

        spawnedWordObjects.Clear();

        WordBehaviour.allWords.Clear();
    }

    public string GetDictionaryAsEditorText()
    {
        string result = "";

        foreach (WordData wd in words)
        {
            result += "[" + wd.mot + " = " + wd.definition + "];\n";
        }

        return result;
    }

    public string GetSavedDictionaryAsEditorText()
{
    string savePath = Path.Combine(
        Application.persistentDataPath,
        "dictionary.xml"
    );

    if (!File.Exists(savePath))
    {
        Debug.LogWarning(
            "Aucun dictionnaire personnalisé sauvegardé. Chargement du dictionnaire actuel."
        );

        return GetDictionaryAsEditorText();
    }

    XmlDocument doc = new XmlDocument();

    try
    {
        string xmlText =
            File.ReadAllText(savePath);

        doc.LoadXml(xmlText);
    }
    catch
    {
        Debug.LogError("Impossible de lire le dictionnaire personnalisé.");

        return GetDictionaryAsEditorText();
    }

    string result = "";

    XmlNodeList wordNodes =
        doc.SelectNodes("//element");

    foreach (XmlNode node in wordNodes)
    {
        if (node["mot"] == null)
            continue;

        if (node["definition"] == null)
            continue;

        string mot =
            node["mot"].InnerText.Trim();

        string definition =
            node["definition"].InnerText.Trim();

        result += "[" + mot + " = " + definition + "];\n";
    }

    return result;
}



    public void ReplaceDictionary(List<WordData> newWords)
    {
        words.Clear();

        foreach (WordData wd in newWords)
        {
            words.Add(wd);
        }

        SaveDictionaryToXML();

        ShuffleWords();

        GenerateWords();
    }

    void SaveDictionaryToXML()
    {
        XmlDocument doc = new XmlDocument();

        XmlElement root =
            doc.CreateElement("dictionary");

        doc.AppendChild(root);

        foreach (WordData wd in words)
        {
            XmlElement element =
                doc.CreateElement("element");

            XmlElement mot =
                doc.CreateElement("mot");

            mot.InnerText = wd.mot;

            XmlElement definition =
                doc.CreateElement("definition");

            definition.InnerText = wd.definition;

            element.AppendChild(mot);
            element.AppendChild(definition);

            root.AppendChild(element);
        }

        doc.Save(SavePath);

        Debug.Log("Dictionnaire sauvegardé dans : " + SavePath);
    }
}
