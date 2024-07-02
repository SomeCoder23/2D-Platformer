using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public float speed = 5f, jumpForce = 8f, mojoPowerBoost = 15f;
    public int powerUpTime = 1000;

    public AudioClip taTing, mojoCollect, eatingAudio, gemAudio, keyAudio, hitAudio, jumpAudio;
    public AudioClip[] throwingAudio = new AudioClip[3];
    public AudioClip[] walkingAudio = new AudioClip[2];
    public int health = 100;
    public GameObject mojoDisk;
    public Transform shootPoint;
    public ParticleSystem dust, puff;

    static int lives = 3, points = 0;

    bool speedOn = false, onGround, jumping = true, climbing = false, swimming = false;
    int powerT, jumps = 0, mojoDisks = 0;
    float jumpBCount;
    PlayerUI ui;
    Vector2 dir = Vector2.right;
    Vector3 movement = new Vector3();
    [HideInInspector]
    public Vector3 checkpoint;
    Animator animator;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    GameObject prevGem, prevKey, prevBounds, prevLava, prevMojo;
    //AudioSource audio;
    SoundManager audio;
    float s;
    int pNum = 1;
    ParticleSystem.EmissionModule footEmission;

    void Start()
    {
        ui = PlayerUI.instance;
        if (gameObject.tag == "Player1") pNum = 1;
        else pNum = 2;
        s = speed;
        movement.z = 0;
        movement.y = 0;
        powerT = powerUpTime;
        checkpoint = transform.position;
        animator = gameObject.GetComponent<Animator>();
        sprite = gameObject.GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        audio = SoundManager.instance;
        points = 0;
        footEmission = dust.emission;
        onGround = true;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal" + pNum.ToString());
        if (movement.x != 0)
        {
            Debug.Log(onGround ? "on ground" : "not on ground");
            if (onGround)
              footEmission.rateOverTime = 35f;

            if (!swimming)
            {
                animator.SetBool("isWalking", true);
            }
            //audio.RandomClip(walkingAudio);
            if (movement.x > 0)
                sprite.flipX = false;
            else if (movement.x < 0)
                sprite.flipX = true;
        }
        else
        {
            animator.SetBool("isWalking", false);
            footEmission.rateOverTime = 0f;
        }

        //if (!climbing)
        //{
            if (Input.GetButtonUp("Jump" + pNum.ToString()) && rb.velocity.y > 0 && jumps < 1)
            {
                //jumps++;
                Debug.Log("**jump number " + jumps);
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                footEmission.rateOverTime = 0f;
                //audio.PlayOneShot(jumpAudio);
            }
            else if (Input.GetButtonDown("Jump" + pNum.ToString()) && (onGround || jumps < 2))
            {
                jumps++;
                Debug.Log("jump number " + jumps);
                footEmission.rateOverTime = 0f;
                //rb.AddForce(Vector2.up * jumpForce * 100);
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * 2f);
                audio.PlayOneShot(jumpAudio);
        }

        //}else if (Input.GetButtonDown("Jump" + pNum.ToString()))
        //{
        //    transform.position += new Vector3(movement.x, Vector2.up.y, 0f) * Time.deltaTime * speed;
        //}

        if (pNum == 1)
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                dir = Vector2.right;
                sprite.flipX = false;
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                dir = Vector2.left;
                sprite.flipX = true;
            }
        }
        else if (pNum == 2)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                dir = Vector2.right;
                sprite.flipX = false;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                dir = Vector2.left;
                sprite.flipX = true;
            }
        }

        if (Input.GetButtonUp("Fire" + pNum.ToString()))
        {
            if(mojoDisks > 0)
            {
                audio.RandomClip(throwingAudio);
                mojoDisks--;
                ui.ChangeMojo(pNum, -1);
                if (dir == Vector2.left)
                    transform.Rotate(new Vector3(0, 180, 0));
                GameObject clone = Instantiate(mojoDisk, shootPoint.transform.position, Quaternion.identity);
                // int dir = movement.x < 0 ? -1 : 1;
                
                clone.GetComponent<Rigidbody2D>().velocity = dir * mojoPowerBoost * Time.deltaTime * 10f; //new Vector2(mojoPowerBoost * Time.fixedDeltaTime * 3 * dir, clone.GetComponent<Rigidbody2D>().velocity.y);
                //clone.GetComponent<Rigidbody2D>().move;
                Debug.Log("shooooooting mojo");
                transform.rotation = Quaternion.identity;
            }
        }
    }

    private void FixedUpdate()
    {
        if (ui.powerOn)
        {
            //int dir = movement.x < 0 ? -1 : 1;
            rb.velocity = dir * mojoPowerBoost * Time.deltaTime * 10f;//new Vector2(mojoPowerBoost * Time.fixedDeltaTime * 3 * dir, rb.velocity.y);
            animator.SetBool("PowerOn", true);
            animator.SetBool("isWalking", false);
            animator.SetBool("isSwimming", false);
            Debug.Log("power onnnnn!!");
            movement.y = Input.GetAxisRaw("Vertical" + pNum.ToString());
            //gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        }
        else
        {
            animator.SetBool("PowerOn", false);
            animator.SetBool("isWalking", true);
            //gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        }
        transform.position += movement * Time.deltaTime * s;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
       

        if(collision.gameObject.tag == "Ground" || (collision.gameObject.GetComponent<Spring>() != null && collision.gameObject.GetComponent<Spring>().item == ItemType.Spring))
        {
            prevLava = collision.gameObject;
            prevBounds = collision.gameObject;
            onGround = true;
            jumps = 0;
            Debug.Log("jumps =  " + jumps);
        }


        else if (collision.gameObject.tag == "Enemy")
        {
            if (ui.powerOn)
                Destroy(collision.gameObject);
            else
            {
                onGround = true;
                jumps = 0;
                if (collision.gameObject.GetComponent<Enemy>() != null)
                    health -= collision.gameObject.GetComponent<Enemy>().power;
                else health -= 10;
                ui.ChangeHealth(health, pNum);
                audio.PlayOneShot(hitAudio);
                if (health <= 0)
                    Die();
            }
            
        }
       
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            onGround = true;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Ground" || (collision.gameObject.GetComponent<Spring>() != null && collision.gameObject.GetComponent<Spring>().item == ItemType.Spring))
        {
            onGround = false;
            Debug.Log("ohoho");
        }
        else if(collision.gameObject.tag == "Ladder")
        {
            climbing = false;
            animator.SetBool("isClimbing", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "bounds")
        {
            if (prevBounds != null && prevBounds == collision.gameObject) return;
            else prevBounds = collision.gameObject;
            Die();
        }

        if (collision.gameObject.tag == "Ladder" && Input.GetButtonUp("Jump" + pNum.ToString()))
        {
            Debug.Log("is climbing");
            animator.SetBool("isClimbing", true);
            animator.SetBool("isWalking", false);
            climbing = true;
        }
        if (collision.gameObject.tag == "mushroom")
        {
            if (health < 100)
            {
                Destroy(collision.gameObject);
                health += 5;
                ui.ChangeHealth(health, pNum);
                audio.PlayOneShot(eatingAudio);
            }

        }
        else if (collision.gameObject.tag == "coin")
        {
            Destroy(collision.gameObject);
            points += 10;
            audio.PlayOneShot(taTing);
            ui.ChangeCoins(points);
        }
        else if (collision.gameObject.tag == "diamond")
        {
            if (prevGem != null && prevGem == collision.gameObject) return;
            else prevGem = collision.gameObject;
            Destroy(collision.gameObject);
            points += 50;
            ui.ChangeCoins(points);
            audio.PlayOneShot(gemAudio);

            if ((ui.player == PlayerTag.Player1 && ui.singlePlayer) || (!ui.singlePlayer && pNum == 1)) 
                ui.UpdateGems(++ui.gGems, 1);
            else if ((ui.player == PlayerTag.Player2 && ui.singlePlayer) || (!ui.singlePlayer && pNum == 2)) 
                ui.UpdateGems(++ui.yGems, 2);

        }
        else if(collision.gameObject.tag == "key")
        {
            if (prevKey != null && prevKey == collision.gameObject) return;
            else prevKey = collision.gameObject;
            Destroy(collision.gameObject);
            ui.gotKey(pNum);
            audio.PlayOneShot(keyAudio);
            Debug.Log("PLayer number " + pNum);
        }
        else if (collision.gameObject.tag == "mojo")
        {
            if (prevMojo != null && prevMojo == collision.gameObject) return;
            else prevMojo = collision.gameObject;
            mojoDisks++;
            Destroy(collision.gameObject);
            ui.ChangeMojo(pNum, 1);
            audio.PlayOneShot(mojoCollect);
        }
        else if(collision.gameObject.GetComponent<Door>() != null)
        {
            Debug.Log("Bumped into Door");
            Door door = collision.gameObject.GetComponent<Door>();
            if (door.requiredItem != Item.Keys) return;
            if (!ui.singlePlayer)
            {
                if (pNum == 1)
                {
                    if (ui.gKeys >= door.requiredNumber)
                    {
                        ui.greenDoorOpen = true;
                        door.OpenDoor(this.gameObject);
                    }
                }
                else if (pNum == 2)
                {
                    if (ui.yKeys >= door.requiredNumber)
                    {
                        ui.yellowDoorOpen = true;
                        door.OpenDoor(this.gameObject);
                    }
                }
            }
            else
            {
                if (ui.player == PlayerTag.Player1 && ui.gKeys >= door.requiredNumber)
                {
                    ui.greenDoorOpen = true;
                    door.OpenDoor(this.gameObject);
                }
                else if (ui.player == PlayerTag.Player2 && ui.yKeys >= door.requiredNumber)
                {
                    ui.yellowDoorOpen = true;
                    door.OpenDoor(this.gameObject);
                }
             }
           }
        //else if (collision.gameObject.GetComponent<Spring>() != null && collision.gameObject.GetComponent<Spring>().
        //{
        //    checkpoint = collision.gameObject.transform.position;
        //}
        else if(collision.gameObject.tag == "lava")
        {
            if (prevLava != null && prevLava == collision.gameObject) return;
            else prevLava = collision.gameObject;

            if (ui.powerOn)
                rb.velocity = new Vector2(rb.velocity.x, 5f);
            else Die();
        }
        else if(collision.gameObject.tag == "water")
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isSwimming", true);
            rb.gravityScale = 0.1f;
            swimming = true;
        }
        else if(collision.gameObject.tag == "fireBall")
        {
            if (ui.powerOn) return;
            health -= 25;
            audio.PlayOneShot(hitAudio);
            ui.ChangeHealth(health, pNum);
            if (health <= 0)
                Die();
        } else if (collision.gameObject.tag == "spikes")
        {
            if (ui.powerOn) return;
            health -= 10;
            ui.ChangeHealth(health, pNum);
            audio.PlayOneShot(hitAudio);
            if (health <= 0)
                Die();
        }
        //else if (collision.gameObject.tag == "EnemyTop")
        //{
        //    Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        //    enemy.hits++;
        //    enemy.health -= (100 / enemy.hitsTillDeath);
        //    if (enemy.hits >= enemy.hitsTillDeath)
        //        enemy.Die();
        //}
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "water")
        {
            animator.SetBool("isSwimming", false);
            rb.gravityScale = 1f;
            swimming = false;
        }
    }



    public void Die()
    {
        lives--;
        Debug.Log(lives + " lives left");
        ui.Die(lives);
        transform.position = checkpoint;
        health = 100;
        ui.ChangeHealth(100, pNum);
        if (!ui.singlePlayer)
        {
            PlayerManager otherPlayer = GameObject.FindGameObjectWithTag("Player" + (pNum == 1 ? "2" : "1")).GetComponent<PlayerManager>();
            otherPlayer.transform.position = otherPlayer.checkpoint;
            otherPlayer.health = 100;
        }
    }

    
    
    
    //private void OnCollisionStay2D(Collision2D collision)
    //{
    //    if (ui.powerOn)
    //        Destroy(collision.gameObject);

    //    else if (collision.gameObject.tag == "Enemy")
    //    {
    //        if (speedOn)
    //            Destroy(collision.gameObject);
    //        else
    //        {
    //            health -= 10;
    //            ui.ChangeHealth(health, pNum);
    //            if (health < 0)
    //            {
    //                if (--lives <= 0)
    //                    Die();
    //            }
    //        }
    //    }

    //}
}
