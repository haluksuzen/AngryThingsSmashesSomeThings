using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Results : MonoBehaviour
{
    Text levelClearedText;
    Text scoreText;
    Text highscoreText;
    Button reset;
    Button back;
    Button next;
    GameObject stars;
    GameObject failed;
    void Start()
    {
        levelClearedText = transform.Find("Header").Find("LevelClearedText").GetComponent<Text>();
        scoreText = levelClearedText.transform.Find("InfoPanel").Find("ScoreText").GetComponent<Text>();
        highscoreText = scoreText.transform.Find("BestScoreText").GetComponent<Text>();
        stars = levelClearedText.transform.Find("InfoPanel").Find("Stars").gameObject;
        failed = levelClearedText.transform.Find("InfoPanel").Find("Failed").gameObject;
        reset = transform.Find("ButtonsPanel").Find("ResetButton").GetComponent<Button>();
        reset.onClick.AddListener(() => FindObjectOfType<UIManager>().Clicked(reset));
        back = transform.Find("ButtonsPanel").Find("BackButton").GetComponent<Button>();
        back.onClick.AddListener(() => FindObjectOfType<UIManager>().Clicked(back));
        next = transform.Find("ButtonsPanel").Find("NextButton").GetComponent<Button>();
        next.onClick.AddListener(() => FindObjectOfType<UIManager>().Clicked(next));
    }
    public void UpdateResults(UIManager manager)
    {
        Level currLevel = manager.Manager.CurrLevel;
        if (manager.Manager.currentGame.numAsteroids > 0)
        {
            currLevel.CurrentScore += (manager.Manager.currentGame.numAsteroids * 10000);
        }
        if (currLevel.CurrentDefeated)
        {
            stars.SetActive(true);
            failed.SetActive(false);
            next.interactable = true;
            levelClearedText.text = "LEVEL CLEARED !";
            if (manager.LevelIndex == GameManager.numLevels - 1)
            {
                levelClearedText.text = "WORLD DEFEATED !";
                next.interactable = false;
            }           
            else
            {
                manager.Manager.CurrWorld.Levels[manager.LevelIndex + 1].Unlocked = true;
            }
            scoreText.text = "SCORE: " + currLevel.CurrentScore;
            if (currLevel.Highscore < currLevel.CurrentScore)
            {
                highscoreText.text = "new highscore !";
                currLevel.Highscore = currLevel.CurrentScore;
            }
            else if(currLevel.Highscore >= currLevel.CurrentScore)
            {
                highscoreText.text = "best " + currLevel.Highscore;
            }
            for (int i = 0; i < stars.transform.childCount; i++)
            {
                stars.transform.GetChild(i).gameObject.SetActive(false);
                if (currLevel.CurrStarScore > i)
                {
                    StartCoroutine(ShowStar(i));                    
                }
            }
        }
        else
        {
            levelClearedText.text = "Level Failed.";
            scoreText.text = "";
            highscoreText.text = "";
            stars.SetActive(false);
            failed.SetActive(true);
            if (!currLevel.Defeated)
            {
                next.interactable = false;
            }
        }
        currLevel.CurrentScore = 0;
    }
    IEnumerator ShowStar(int index)
    {
        yield return new WaitForSeconds(index * 0.4f);
        stars.transform.GetChild(index).gameObject.SetActive(true);

    }
}
