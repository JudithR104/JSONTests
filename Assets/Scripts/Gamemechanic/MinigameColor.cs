using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;


public class MinigameColor : MonoBehaviour
{
    public Dictionary<int, string> rndTitles = new Dictionary<int, string>();
    public Dictionary<int, Color32> rndColors = new Dictionary<int, Color32>();

    public List<GameObject> answersObjects;
    public Text feedbackText;
    public Text scoreText;
    public GameObject feedbackHolder;
    public float duration = 2f;
    private float durationremaining;

    private int score;

    public MinigameAnswer firstSelectedAnswer;
    public MinigameAnswer secondSelectedAnswer;
    
    public static System.Random rngTitle = new System.Random();
    public static System.Random rngColor = new System.Random();
    public void CreateGame(DataService dataService)
    {
        durationremaining = duration;
        foreach (var val in dataService.data.Values)
        {
            rndTitles.Add(val.id, val.title);
            rndColors.Add(val.id, val.color);
        }

        rndTitles = rndTitles.OrderBy(x => rngTitle.Next()).ToDictionary(item => item.Key, item => item.Value);
        rndColors = rndColors.OrderBy(x => rngTitle.Next()).ToDictionary(item => item.Key, item => item.Value);

        for (int i = 0; i < 15; i++)
        {
            answersObjects[i].GetComponent<MinigameAnswer>().idImage = rndColors.ElementAt(i).Key;
            answersObjects[i].GetComponent<MinigameAnswer>().idTitle = rndTitles.ElementAt(i).Key;
            answersObjects[i].GetComponentInChildren<Text>().text = rndTitles.ElementAt(i).Value;
            answersObjects[i].transform.GetChild(0).GetComponent<Image>().color = rndColors.ElementAt(i).Value;
        }

        score = 0;
        scoreText.text = $"{score}";
    }

    private void Update()
    {
        if (feedbackHolder.activeSelf)
        {
            durationremaining -= Time.deltaTime;
            if (durationremaining <= 0)
            {
                durationremaining = duration;
                feedbackHolder.SetActive(false);
            }
        }
    }

    public void OnAnswer(MinigameAnswer answer)
    {
        if (feedbackHolder.activeSelf)
        {
            return;
        }
        if (!firstSelectedAnswer )
        {
            firstSelectedAnswer = answer;
            return;
        }

        secondSelectedAnswer = answer;

        if (firstSelectedAnswer.idTitle == secondSelectedAnswer.idImage ||
            firstSelectedAnswer.idImage == secondSelectedAnswer.idTitle)
        {
            CorrectAnswer(firstSelectedAnswer.idTitle == secondSelectedAnswer.idImage);
        }
        else
        {
            WrongAnswer();
        }

        firstSelectedAnswer = null;
        secondSelectedAnswer = null;
        if (CheckGameOver())
        {
            feedbackHolder.SetActive(true);
            feedbackText.text = "Gewonnen!";
        }
    }

    private void CorrectAnswer(bool titleOfFirstAnswer)
    {
        if (titleOfFirstAnswer)
        {
            firstSelectedAnswer.gameObject.GetComponentInChildren<Text>().text = "";
            secondSelectedAnswer.transform.GetChild(0).GetComponent<Image>().enabled = false;
            firstSelectedAnswer.solvedTitle = true;
            secondSelectedAnswer.solvedColor = true;
        }
        else
        {
            secondSelectedAnswer.gameObject.GetComponentInChildren<Text>().text = "";
            firstSelectedAnswer.transform.GetChild(0).GetComponent<Image>().enabled = false;
            secondSelectedAnswer.solvedTitle = true;
            firstSelectedAnswer.solvedColor = true;
        }

        score += 10;
        scoreText.text = $"{score}";
        
        feedbackHolder.SetActive(true);
        feedbackText.text = "Richtig!";
    }

    private void WrongAnswer()
    {
        if (score >= 5)
        {
            score -= 5;
        }
        scoreText.text = $"{score}";
        
        feedbackHolder.SetActive(true);
        feedbackText.text = "Leider falsch...";
    }

    private bool CheckGameOver()
    {
        foreach (var value in answersObjects)
        {
            if (value.GetComponent<MinigameAnswer>().solvedColor == false ||
                value.GetComponent<MinigameAnswer>().solvedTitle == false)
            {
                return false;
            }
        }

        return true;
    }
    
}
