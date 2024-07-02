using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction;
    public float turnTime;
    public bool destroyable = false;
    public float timeTillGone = 2f;
    public bool needTrigger = false, moving = true;
    public Spring lever;

    Rigidbody2D rb;
    float turnCount;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (needTrigger)
            if (!lever.turnedOn) return;

        if (!moving) Destroy(gameObject);

        rb.velocity = direction * speed * Time.deltaTime;
        if (turnCount > 0) turnCount -= Time.deltaTime;
        else
        {
            turnCount = turnTime;
            speed *= -1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (needTrigger)
            if (!lever.turnedOn) return;

        string tag = collision.gameObject.tag;
        if (tag == "Player1" || tag == "Player2")
        {
            if (destroyable)
                Destroy(gameObject, timeTillGone);
        }
    }
}
