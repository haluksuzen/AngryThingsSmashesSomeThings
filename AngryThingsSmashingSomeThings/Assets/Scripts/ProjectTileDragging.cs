using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ProjectTileDragging : MonoBehaviour
{
    private bool clickedOn;
    private SpringJoint2D spring;
    private Vector2 prevVelocity;
    private Ray leftCatapultToProjectTile;
    private float circleRadius;
    private Ray rayToMouse;
    private Transform catapult;
    private float maxStretchSqr;
    public float maxStretch = 5.0f;
    public LineRenderer catapultLineFront;
    public LineRenderer catapultLineBack;
    void Start()
    {
        StartInitialization();
    }
    void StartInitialization()
    {
        spring = GetComponent<SpringJoint2D>();
        catapult = GameObject.Find("Game").transform.Find("Catapult");
        catapultLineBack = catapult.GetComponent<LineRenderer>();
        catapultLineFront = catapult.GetChild(0).GetComponent<LineRenderer>();
        spring.connectedBody = catapult.GetComponent<Rigidbody2D>();
        LineRendererSetUp();
        rayToMouse = new Ray(catapult.position,Vector3.zero);
        leftCatapultToProjectTile = new Ray(catapultLineFront.transform.position, Vector3.zero);
        CircleCollider2D circle = GetComponent<CircleCollider2D>();
        circleRadius = circle.radius;
        maxStretchSqr = maxStretch * maxStretch;
    }
    void LineRendererSetUp()
    {
        catapultLineFront.SetPosition(0,catapultLineFront.transform.position);
        catapultLineBack.SetPosition(0,catapultLineBack.transform.position);

        catapultLineFront.sortingLayerName = "ForeGround";
        catapultLineBack.sortingLayerName = "ForeGround";

        catapultLineBack.enabled = true;
        catapultLineFront.enabled = true;

        catapultLineFront.sortingOrder = 3;
        catapultLineBack.sortingOrder = 1;
    }
    void Update()
    {
        UpdateProject();   
    }

    void UpdateProject()
    {
        if (Input.GetButtonDown("Jump"))
        {
            SceneManager.LoadScene(0);
        }
        if (clickedOn)
        {   
            Dragging();
        }
        if (spring != null)
        {
            if (!GetComponent<Rigidbody2D>().isKinematic && prevVelocity.sqrMagnitude > GetComponent<Rigidbody2D>().velocity.sqrMagnitude)
            {
                Destroy(spring);
                GetComponent<Rigidbody2D>().velocity = prevVelocity;
            }  
            if (!clickedOn)
            {
                prevVelocity  = GetComponent<Rigidbody2D>().velocity;
            } 
            LineRendererUpDate();
        }
        else
        {
            catapultLineBack.enabled = false;
            catapultLineFront.enabled = false; 
        }      
    }
    void OnMouseDown()
    {
        spring.enabled = false;
        clickedOn = true;
    }
    void OnMouseUp()
    {
        spring.enabled = true;
        GetComponent<Rigidbody2D>().isKinematic = false;
        clickedOn = false;
    }
    void LineRendererUpDate()
    {
        Vector2 catapultToProjectTile = transform.position - catapultLineFront.transform.position;
        leftCatapultToProjectTile.direction = catapultToProjectTile; 

        Vector3 holdPoint = leftCatapultToProjectTile.GetPoint(catapultToProjectTile.magnitude + circleRadius);

        catapultLineFront.SetPosition(1,holdPoint);
        catapultLineBack.SetPosition(1,holdPoint);
    }
    void Dragging()
    {
        Vector3 mouseWorldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 catapultToMouse = mouseWorldPoint - catapult.position;
        if (catapultToMouse.sqrMagnitude > maxStretchSqr)
        {
            rayToMouse.direction = catapultToMouse;
            mouseWorldPoint = rayToMouse.GetPoint(maxStretch);
        }
        mouseWorldPoint.z = 0f;
        transform.position = mouseWorldPoint;

    }
}
