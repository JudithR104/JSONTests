using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public TextAsset contentJson;
    public DataService dataService;
    public MinigameColor minigame;
    void Start()
    {
        dataService = new DataService();
        dataService.Init(contentJson.text);
        minigame.CreateGame(dataService);
    }
    
}
