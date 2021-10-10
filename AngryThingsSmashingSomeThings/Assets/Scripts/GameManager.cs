using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Game game1Prefab;
    [SerializeField] Game game2Prefab;
    [SerializeField] Game game3Prefab;
    public const int numWorlds = 3;
    public const int numLevels = 3;
    public List<World> allWorlds;
    public static Level currLevel;
    public static World currWorld;
    public Game currentGame;
    public CinemachineManager cinemachineManager;
    // Start is called before the first frame update
    void Start()
    {
        cinemachineManager = FindObjectOfType<CinemachineManager>();
        allWorlds = Information.Load();
    }
    void Update()
    {
        if (currentGame != null)
        {
            CheckForCurrentGameOver();
        }
    }
    
    void OnApplicationQuit() 
    {
        Information.Save(allWorlds);    
    }
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
        allWorlds = Information.Load();
    }
    public World CurrWorld
    {
        set
        {
            Debug.Log("yok amk");
            currWorld = value;
            Debug.Log("Current World Set");
        }
        get
        {
            if (currWorld != null)
            {
                return currWorld;
            }
            else
            {
                Debug.Log("Cant Retrieve current world. Value is null");  
                return null;  
            }
        }
    }
    public Level CurrLevel
    {
        set
        {
            currLevel = value;
            Debug.Log("Current level Set");
        }
        get
        {
            if (currLevel != null)
            {
                return currLevel;
            }
            else
            {
                Debug.Log("Cant Retrieve current level. Value is null");  
                return null;  
            }
        }
    }
    void RunCinemachinePolygonTaker()
    {
        cinemachineManager.TakePolygonCollider2D();
    }

    void CheckForCurrentGameOver()
    {
        if (currentGame.currLevelObj == null && currentGame.gameOver)
        {
            Debug.Log("Game Over!!!");
        }
    }
   public void StartGame(int level, int world)
   {
        if (world == 0)
        {
            currentGame = Instantiate(game1Prefab) as Game;
            currentGame.gameObject.name = "Game";
            level +=3;
        }
        else if (world == 1)
        {
            currentGame = Instantiate(game2Prefab) as Game;
            currentGame.gameObject.name = "Game";
            level += 6;
        }
        else if (world == 2)
        {
            currentGame = Instantiate(game3Prefab) as Game;
            currentGame.gameObject.name = "Game";
        }
        //level ile world 0 geliyor 
        if (!Game.isMenuOpn)
        {
            currentGame.InitLevel(level, 3);   
        }
       Invoke("RunCinemachinePolygonTaker",.1f);
   }
}
