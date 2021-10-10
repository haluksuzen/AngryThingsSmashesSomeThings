using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CinemachineManager : MonoBehaviour
{
    private GameObject asteroidForFollow;
    private PolygonCollider2D asteroidBoundriesBoxCollider;
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineConfiner cinemachineConfiner;
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineConfiner = GetComponent<CinemachineConfiner>();
        //Invoke("TakePolygonCollider2D",.1f);
    }

    void Update()
    {
        if (asteroidForFollow == null)
        {
            if (Game.CheckMovemenetStoped())
            {
               asteroidForFollow = GameObject.Find("Asteroid(Clone)"); 
            }
            if (asteroidForFollow != null)
            {
                virtualCamera.LookAt = asteroidForFollow.transform;
                virtualCamera.Follow = asteroidForFollow.transform;   
            }
        }   
    }
    public void TakePolygonCollider2D()
    {
        asteroidBoundriesBoxCollider = GameObject.Find("Game").transform.Find("CameraBoundries").GetComponent<PolygonCollider2D>();
        cinemachineConfiner.m_BoundingShape2D = asteroidBoundriesBoxCollider;
    }
}
