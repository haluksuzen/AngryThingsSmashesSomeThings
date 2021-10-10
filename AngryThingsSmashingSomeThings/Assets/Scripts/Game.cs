using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] GameObject asteroidPrefab;
    [SerializeField] GameObject[] levelPrefabs;
    public GameObject currLevelObj;
    public int numAsteroids;
    public int EnemiesRemaining;
    public bool gameOver = false;
    public static bool isMenuOpn;
    private Transform start;
    private GameObject asteroid;
    void Start()
    {
        start = GameObject.Find("Game").transform.Find("Start");
        asteroid = GameObject.FindGameObjectWithTag("AsteroidTag");
    }
    void Update()
    {
        
        UpdateGameOver();
        UpdateAsteroid();  
        GoMenu(); 
    }
   
    void UpdateGameOver()
    {
        EnemiesRemaining = GameObject.FindGameObjectsWithTag("EnemyTag").Length;
        if (EnemiesRemaining == 0 && currLevelObj && !gameOver && !asteroid)
        {
            if (CheckMovemenetStoped())
            {
                StartCoroutine(WaitToDestroy(0.8f));
            }
        }
    }
    void GoMenu()
    {
        if (Input.GetKey(KeyCode.Backspace))
        {
            StartCoroutine(WaitToDestroy(0));   
            isMenuOpn = true;
        }
    }
    public IEnumerator WaitToDestroy(float t)
    {
        yield return new WaitForSeconds(t);
        if (CheckMovemenetStoped())
        {
            gameOver = true;
            Debug.Log("clearing");
            Destroy(asteroid.gameObject);
            Destroy(currLevelObj.gameObject);
        }else
        {
            Debug.Log("still moving bitch...");
        }
    }
    public void InitLevel(int level, int numasteroids)
    {   
        if (currLevelObj == null && GameObject.FindGameObjectWithTag("LevelTag") == null)
        {
            if (!gameOver)
            {
                numAsteroids = numasteroids;
                currLevelObj = Instantiate(levelPrefabs[level]) as GameObject;
                currLevelObj.transform.SetParent(transform);   
            }
        }
    }

    // Update is called once per frame
    void UpdateAsteroid()
    {
        if (asteroid == null && GameObject.FindGameObjectWithTag("AsteroidTag") == null)
        {
            if (!gameOver && EnemiesRemaining > 0 && numAsteroids > 0)
            {
                numAsteroids--;
                asteroid = Instantiate(asteroidPrefab,start.position,start.rotation) as GameObject;
            }
            else if(numAsteroids == 0)
            {
                StartCoroutine(WaitToDestroy(0));
            }
            
        }
    }
    public static bool CheckMovemenetStoped()
    {
        Rigidbody2D[] bodies = FindObjectsOfType<Rigidbody2D>();
        List<Rigidbody2D> checkBodies = new List<Rigidbody2D>();
        foreach (Rigidbody2D body in bodies)
        {
            if (body.position.y > -8 && body.gameObject.tag == "AsteroidTag")
            {
                checkBodies.Add(body);
            }
        }
        int count = 0;
        int compare = checkBodies.Count;
        foreach (Rigidbody2D body in checkBodies)
        {
            if (body.velocity.magnitude <= 0.025f )
            {
                count++;
            }
        }
        if (count == compare)
        {
            return true;
        }
        return false;
    }
    // kuş ölünce başlatmada sıkıntı var 

}
