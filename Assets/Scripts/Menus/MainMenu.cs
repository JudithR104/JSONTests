using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject highscoreHolders;
    public GameObject highscorePanel;
    public GameObject mainPanel;
    public TextAsset highscoresJSON;

    public List<string> highscoreList;
    
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ShowHighscores()
    {
        highscorePanel.SetActive(true);
       // mainPanel.SetActive(false);
        GetHighscores();
        if (highscoreList.Count > 0)
        {
            for (int i = 0; i < highscoreList.Count; i++)
            {
                highscoreHolders.transform.GetChild(i).GetComponent<Text>().text = highscoreList[i];
            }
        }
    }

    public void ShowMain()
    {
        highscorePanel.SetActive(false);
        //mainPanel.SetActive(true);
        SaveHighscores();
    }

    public void ExitGame()
    {
        Application.Quit();
        SaveHighscores();
    }

    private void GetHighscores()
    {
        JSONObject highscores = new JSONObject(highscoresJSON);
        foreach (var val in highscores.list)
        {
            highscoreList.Add(val.str);
        }
    }

    private void SaveHighscores()
    {
        JSONObject highscores = new JSONObject();
        foreach (var val in highscoreList)
        {
            highscores.Add(val);
        }

        File.WriteAllText(AssetDatabase.GetAssetPath(highscoresJSON), highscores.ToString());
        EditorUtility.SetDirty(highscoresJSON);
    }
    
}
