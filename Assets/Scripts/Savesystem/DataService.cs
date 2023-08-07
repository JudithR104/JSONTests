using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataService 
{
    public Dictionary<int, Answerdata> data = new Dictionary<int, Answerdata>();
    public string fileContent;
    private JSONObject contentJson;
    
    public void Init(string jsonString)
    {
        fileContent = jsonString;
         int counter = 0;
         contentJson = new JSONObject(fileContent);
         foreach (var value in contentJson.GetField("Answers").list)
         {
             var title = value["title"].str;
             Answerdata newAnswer = new Answerdata(counter, title);
             newAnswer.SetColor(value["color"]["r"].n, value["color"]["g"].n, value["color"]["b"].n );
             data.Add(counter, newAnswer);
             counter++;
             Debug.Log(newAnswer.title);
         }
    }
}

public class Answerdata
{
    public int id;
    public string title;
    public Color32 color;

    public Answerdata(int id, string title)
    {
        this.id = id;
        this.title = title;
        color = new Color32(0,0,0, 250);
    }

    public void SetColor(float r, float g, float b)
    {
        color = new Color32((byte)r, (byte)g, (byte)b, 250);
    }
}