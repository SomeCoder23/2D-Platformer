using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone : MonoBehaviour
{
    Animator animation;
    SpriteRenderer sprite;
    Transform enemy;
    Enemy e;
    float distance;
    public float damage = 25;
    public float livingTime = 15f;
    bool touching = false;
    private void Start()
    {
        animation = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(enemy != null)
        {
            distance = Vector2.Distance(transform.position, enemy.position);
            transform.position = Vector2.MoveTowards(transform.position, enemy.position, 7f * Time.deltaTime);
            if (enemy.position.x > transform.position.x)
            sprite.flipX = false;
        else sprite.flipX = true;
        }

        if (livingTime > 0) livingTime -= Time.deltaTime;
        else Destroy(gameObject);
        



    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            enemy = collision.gameObject.GetComponent<Transform>();
            
            animation.enabled = true;
            animation.SetBool("isWalking", true);
            distance = Vector2.Distance(transform.position, enemy.position);
            if (collision.gameObject.GetComponent<Enemy>() != null)
            {
                e = collision.gameObject.GetComponent<Enemy>();
                e.health -= damage;
                e.healthBar.SetHealth(e.health);

            }

            Debug.Log("collidded with enemyyyyyyyy");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<Enemy>() != null)
            {
                e = collision.gameObject.GetComponent<Enemy>();
                e.health -= damage;
                e.healthBar.SetHealth(e.health);

            }
        }
    }



    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Enemy")
    //    {
    //        enemy = collision.gameObject.GetComponent<Transform>();
    //        animation.enabled = true;
    //        animation.SetBool("isWalking", true);
    //        distance = Vector2.Distance(transform.position, enemy.position);
    //        if (collision.gameObject.GetComponent<Enemy>() != null)
    //        {
    //            collision.gameObject.GetComponent<Enemy>().health -= damage;
    //        }

    //        Debug.Log("collidded with enemyyyyyyyy");
    //    }
    //}
}
