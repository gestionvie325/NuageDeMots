using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;

public class DictionaryEditorUI : MonoBehaviour
{
    public static DictionaryEditorUI instance;

    [Header("UI")]
    public GameObject editorPanel;
    public TMP_InputField editorInputField;

    void Awake()
    {
        instance = this;

        if (editorPanel != null)
        {
            editorPanel.SetActive(false);
        }
    }

    public void OpenEditor()
    {
        if (editorPanel == null)
        {
            Debug.LogError("editorPanel n'est pas assigné.");
            return;
        }

        if (editorInputField == null)
        {
            Debug.LogError("editorInputField n'est pas assigné.");
            return;
        }

        if (WordCloudManager.instance == null)
        {
            Debug.LogError("WordCloudManager.instance est null.");
            return;
        }

        editorInputField.text =
            WordCloudManager.instance.GetDictionaryAsEditorText();

        editorPanel.SetActive(true);
    }

    public void CloseEditor()
    {
        if (editorPanel != null)
        {
            editorPanel.SetActive(false);
        }
    }

    public void SaveEditor()
    {
        if (editorInputField == null)
        {
            Debug.LogError("editorInputField n'est pas assigné.");
            return;
        }

        if (WordCloudManager.instance == null)
        {
            Debug.LogError("WordCloudManager.instance est null.");
            return;
        }

        string editorText = editorInputField.text;

        List<WordData> parsedWords =
            ParseEditorText(editorText);

        if (parsedWords.Count == 0)
        {
            Debug.LogError("Aucun mot valide trouvé dans l'éditeur.");
            return;
        }

        WordCloudManager.instance.ReplaceDictionary(parsedWords);

        Debug.Log("Dictionnaire enregistré depuis l'éditeur.");

        CloseEditor();
    }

    public void LoadLiteratureDictionary()
    {
        LoadDictionaryFromResources("dictionary_litterature");
    }

    public void LoadPhilosophyDictionary()
    {
        LoadDictionaryFromResources("dictionary_philosophie");
    }

    public void LoadCustomDictionary()
    {
        if (WordCloudManager.instance == null)
        {
            Debug.LogError("WordCloudManager.instance est null.");
            return;
        }

        editorInputField.text =
            WordCloudManager.instance.GetSavedDictionaryAsEditorText();
    }

    void LoadDictionaryFromResources(string resourceName)
    {
        if (editorInputField == null)
        {
            Debug.LogError("editorInputField n'est pas assigné.");
            return;
        }

        TextAsset xmlFile =
            Resources.Load<TextAsset>(resourceName);

        if (xmlFile == null)
        {
            Debug.LogError(
                "Impossible de charger le dictionnaire Resources : "
                + resourceName
            );

            return;
        }

        string editorText =
            ConvertXmlToEditorText(xmlFile.text);

        editorInputField.text = editorText;

        Debug.Log("Dictionnaire chargé dans l'éditeur : " + resourceName);
    }

    string ConvertXmlToEditorText(string xmlText)
    {
        string result = "";

        XmlDocument doc = new XmlDocument();

        try
        {
            doc.LoadXml(xmlText);
        }
        catch
        {
            Debug.LogError("XML invalide.");
            return "";
        }

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

    List<WordData> ParseEditorText(string text)
    {
        List<WordData> result =
            new List<WordData>();

        Regex regex = new Regex(
            @"\[(.*?)=(.*?)\]\s*;?",
            RegexOptions.Singleline
        );

        MatchCollection matches =
            regex.Matches(text);

        foreach (Match match in matches)
        {
            string mot =
                match.Groups[1].Value.Trim();

            string definition =
                match.Groups[2].Value.Trim();

            if (string.IsNullOrWhiteSpace(mot))
                continue;

            if (string.IsNullOrWhiteSpace(definition))
                continue;

            WordData wd = new WordData();

            wd.mot = mot;
            wd.definition = definition;

            result.Add(wd);
        }

        return result;
    }
}
