using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YellowEnemyDamage : MonoBehaviour
{
    public Sprite damagedSprite;

    private SpriteRenderer spriteRenderer;
    private GameManager manager;
    private bool isYellowBirdShooted = true;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        manager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.tag !="PlankTag")
        {
            return;
        }
        
        if (isYellowBirdShooted)
        {
            spriteRenderer.sprite = damagedSprite;
            isYellowBirdShooted = false;
            return;
        }
        if (other.gameObject.tag == "PlankTag")
        {
            Kill();
        }
    }
    void Kill()
    {
        spriteRenderer.enabled = false;
        manager.CurrLevel.CurrentScore += 8000;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().isKinematic = true;
        Destroy(this.gameObject,0.5f);
        GetComponent<ParticleSystem>().Play();
    }
}
