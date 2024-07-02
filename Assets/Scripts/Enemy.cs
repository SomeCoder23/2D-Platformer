using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Enemy : MonoBehaviour
{
    //public LayerMask mask;
    //public Transform maskCheck;
    public bool Patrol = false, chasePlayer = false;
    public float speed = 10f, flipTime = 4.5f, chasingDistance = 0, chasingSpeed = 4f;
    public Sprite deadSprite;
    public PlayerTag player;
    public float maxHealth = 100;
    public EnemyHealth healthBar;
    public bool selfHealing = false;
    public int rewardsCount = 0, power = 10;
    public GameObject[] rewards;
    public int hitsTillDeath = 1;
    public AudioClip dyingAudio;
    public float dieTime = 2f;
    //public Transform startPos;

    bool flip = false, patrol, die = false, flipX = true, chasing = false;
    float flipCount, distance, healTime = 25;
    [HideInInspector]
    public float health;
    float s;
    string playerTag;
    Rigidbody2D rb;
    Transform playerPos;
    Vector3 startPos;
    SpriteRenderer sprite;
    Animator animator;
    SoundManager audio;
    [HideInInspector]
    public int hits = 0;
  
    
    void Start()
    {
        if (die) return;
        if(healthBar != null)
          healthBar.SetMaxHealth(maxHealth);
        health = maxHealth;
        audio = SoundManager.instance;
        //rewards = new GameObject[rewardsCount];
        sprite = gameObject.GetComponent<SpriteRenderer>();
        animator = gameObject.GetComponent<Animator>();
        startPos = transform.position;
        s = speed;
        patrol = Patrol;
        if (PlayerUI.instance.singlePlayer)
            playerTag = "Player2";
        else
        {
            if (player == PlayerTag.Player1)
                playerTag = "Player1";
            else playerTag = "Player2";
        }
        if (patrol || chasePlayer)
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            flipCount = flipTime;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (patrol)
        {
            if(flipCount > 0)
                flipCount -= Time.deltaTime;
            else
            {
                flipCount = flipTime;
                flip = true;
            }
        }
        if (chasePlayer)
        {
            playerPos = GameObject.FindGameObjectWithTag(playerTag).transform;
            distance = Vector2.Distance(transform.position, playerPos.position);
            if (distance <= chasingDistance)
            {
                patrol = false;
                chasing = true;
                transform.position = Vector2.MoveTowards(transform.position, playerPos.position, chasingSpeed * Time.deltaTime);
                if (playerPos.position.x > transform.position.x)
                {
                    flipX = true;
                    sprite.flipX = flipX;
                    s = speed;
                }
                else
                {
                    flipX = false;
                    sprite.flipX = flipX;
                    s = speed * -1;
                }
            }
            else patrol = Patrol; 

            
            if (selfHealing && health < 100)
            {
                if (healTime > 0) healTime -= Time.deltaTime;
                else
                {
                    health++;
                    healTime = 25f;
                    if (healthBar != null)
                      healthBar.SetHealth(health);
                }
            }
            
            //else if (!patrol)
            //{
            //    transform.position = Vector2.MoveTowards(transform.position, startPos.position, 3 * Time.deltaTime);
            //    if (startPos.position.x > transform.position.x)
            //    {
            //        flipX = true;
            //        sprite.flipX = true;
            //    }
            //    else
            //    {
            //        flipX = false;
            //        sprite.flipX = false;
            //    }
            //    if (transform.position == startPos.position)
            //        patrol = true;
            //}
            //else
            //    sprite.flipX = false;
            //else if (chasing)
            //{
            //    Debug.Log("Moving towards start Position");
            //    transform.position = Vector2.MoveTowards(transform.position, startPos.position, 3 * Time.deltaTime);
            //    //if (startPos.position.x > transform.position.x)
            //    //{
            //    //    sprite.flipX = true;
            //    //    flipX = true;
            //    //}
            //    //else
            //    //{
            //    //    sprite.flipX = false;
            //    //    flipX = false;
            //    //}

            //    if (transform.position == startPos.position)
            //    {
            //        Debug.Log("returned to start position");
            //        chasing = false;
            //        patrol = Patrol;
            //    }
            //}
            //else if (transform.position == startPos)
            //{
            //    Debug.Log("returned to start position");
            //    chasing = false;
            //    patrol = Patrol;
            //}

            //else if (distance <= retreatingDistance && distance > chasingDistance)
            //{
            //    patrol = false;
            //    transform.position = Vector2.MoveTowards(transform.position, startPos, 3 * Time.deltaTime);
            //    if (startPos.x > transform.position.x)
            //        sprite.flipX = true;
            //    else sprite.flipX = false;
            //}
            //else /*if (transform.position == startPos)*/
            //    patrol = Patrol;

        }

        if (die)
        {
            if (dieTime > 0) dieTime -= Time.deltaTime;
            else
            {
                die = false;
                for (int i = 0; i < rewardsCount; i++)
                    Instantiate(rewards[i], transform.position + new Vector3(i, i, 0), Quaternion.identity);

                gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (die) return;
        if (health <= 0) Die();
 
        if (patrol)
        {
            if (flip)
               Flip();
            rb.velocity = new Vector2(s * Time.fixedDeltaTime * 3, rb.velocity.y);
            //flip = Physics2D.OverlapCircle(maskCheck.position, 0.1f, mask);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(animator.parameterCount > 0)
        {
            if(collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
            {
                animator.SetBool("Hit", true);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (animator.parameterCount > 0)
        {
            if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
            {
                animator.SetBool("Hit", false);
            }
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if(collision.gameObject.tag == "Weapon")
    //    {
    //        Debug.Log("enemy hit");
    //        //health -= collision.gameObject.GetComponent<Weapon>().damage;
    //        if (health <= 0)
    //            Die();
    //    }
    //}

    public void Die()
    {
        Debug.Log(gameObject.name + " is dying");
        if(animator != null)
          animator.enabled = false;
        die = true;
        patrol = false;
        chasePlayer = false;
        rb.gravityScale = 1f;
        sprite.sprite = deadSprite;
    }


    void Flip()
    {
        patrol = false;
        //transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        sprite.flipX = !flipX;
        flipX = !flipX;
        s *= -1;
        patrol = true;
        flip = false;
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, chasingDistance);
        //Gizmos.DrawWireSphere(transform.position, retreatingDistance);
    }
}

public enum PlayerTag
{
    Player1, 
    Player2
}
