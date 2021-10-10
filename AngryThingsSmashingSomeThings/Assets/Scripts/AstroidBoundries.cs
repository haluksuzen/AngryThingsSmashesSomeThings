using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class AstroidBoundries : MonoBehaviour
{
    public Rigidbody2D asteroid;
    public float resetSpeed = 0.025f;
    private SpringJoint2D spring;
    private float resetSpeedSqr;
    private GameManager manager;
    
    void Start()
    {
        resetSpeedSqr = resetSpeed * resetSpeed;       
        manager = FindObjectOfType<GameManager>();
    } 
    void Update()
    {
        if (asteroid)
        {
            if (!spring)
            {
                spring = asteroid.GetComponent<SpringJoint2D>();
            }
            CheckForReset();
        }
        else
        {
            TryFindAsteroid();
        }
    }
    void TryFindAsteroid()
    {
        asteroid =GameObject.FindGameObjectWithTag("AsteroidTag").GetComponent<Rigidbody2D>();
        spring = asteroid.GetComponent<SpringJoint2D>();
    }
    void CheckForReset()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            manager.CurrLevel.CurrentScore += 10000;
            GameReset();
        }
        if (asteroid.velocity.sqrMagnitude < resetSpeedSqr && spring == null)
        {
            GameReset();
        }  
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (asteroid && other.GetComponent<Rigidbody2D>() == asteroid)
        {
            GameReset();
        }
    }
   private void GameReset()
   {
       Destroy(asteroid.gameObject);
   }
}
