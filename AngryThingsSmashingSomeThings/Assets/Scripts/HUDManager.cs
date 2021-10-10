using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    Text levelText;
    Text scoreText;
    Text highscoreText;
    GameObject asteroidDisplay;
    
    public GameObject asteroidPrefab;
    UIManager mgr;

    void Start()
    {
        mgr = FindObjectOfType<UIManager>();
        scoreText = transform.Find("Score").GetComponent<Text>();
        highscoreText = scoreText.transform.Find("Highscore").GetComponent<Text>();
        levelText = transform.Find("Level").GetComponent<Text>();
        asteroidDisplay = transform.Find("Projectiles").gameObject;
    }
    public void UpdateHUD(int score, int highscore, int level, int numasteroids)
    {
        scoreText.text = "Score\n" + score.ToString();
        if (highscore > 0)
        {
            highscoreText.text = "highscore\n" + highscore.ToString();
        }
        else
        {
            highscoreText.text = "";
        }
        levelText.text = "LEVEL: " + level.ToString();
        UpdateAsteroids(numasteroids);
    }
    void UpdateAsteroids(int count)
    {
        int currentDisplayed = asteroidDisplay.transform.childCount;
        if (count != currentDisplayed)
        {
            if (count < currentDisplayed)
            {
                for (int i = currentDisplayed; i > count ; i--)
                {
                    Destroy(asteroidDisplay.transform.GetChild( i - 1).gameObject);
                }
            }
            else if( count > currentDisplayed)
            {
                for (int i = currentDisplayed; i < count; i++)
                {
                    GameObject temp = mgr.InitUIElement(asteroidPrefab, asteroidDisplay.transform);
                    RectTransform rect = temp.GetComponent<RectTransform>();
                    rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 50 * i, 50);
                    rect.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 50);
                }
            }
        }
    }
}
