using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spring : MonoBehaviour
{
    public ItemType item;
    public float force = 5f;
    public Sprite on, off;
    public SpriteRenderer sprite;
    public GameObject reward;
    public AudioClip audio;
    public GameObject leverObject;
    Rigidbody2D rbPlayer;
    bool change = false;
    float changeCount = .5f;
    int hits = 0;
    bool spring = false, box = false, button = false, flag = false, checkPoint = false, lever = false;
    PlayerUI ui;

    [HideInInspector]
    public bool turnedOn = false, turnedOff = true;

    // Start is called before the first frame update
    void Start()
    {
        switch (item)
        {
            case ItemType.Box: box = true; break;
            case ItemType.Spring: spring = true; break;
            case ItemType.Button: button = true; break;
            case ItemType.Flag: flag = true; break;
            case ItemType.CheckPoint: checkPoint = true; break;
            case ItemType.Lever: lever = true; break;
        }
        ui = PlayerUI.instance;
    }

    private void Update()
    {
        if (change && spring)
        {
            if (changeCount > 0)
                changeCount -= Time.deltaTime;
            else
            {
                change = false;
                sprite.sprite = off;
                changeCount = .5f;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collided with " + collision.gameObject.name);
        string tag = collision.gameObject.tag;
        if(audio != null)
            SoundManager.instance.PlayOneShot(audio);

        if (tag == "Player1" || tag == "Player2")
        {
            if (spring) return;
            
           else if (flag || checkPoint)
            {
                gameObject.GetComponent<Animator>().enabled = true;
                if (flag)
                {
                    if (!ui.singlePlayer)
                    {
                        if (tag == "Player1") ui.greenDone = true;
                        else ui.yellowDone = true;
                    }
                    else
                    {
                        if(ui.player == PlayerTag.Player1)
                            ui.greenDone = true;
                        else ui.yellowDone = true;
                    }
                }
                if (checkPoint)
                    collision.gameObject.GetComponent<PlayerManager>().checkpoint = transform.position;
            }          
             //ELSE do whatever pressing the button does;
        }else if(tag == "Bottom")
        {
            sprite.sprite = on;
            if (spring)
            {                
                change = true;
                //animator.enabled = true;
                rbPlayer = collision.gameObject.transform.parent.gameObject.GetComponent<Rigidbody2D>();
                rbPlayer.AddForce(Vector2.up * force * 100);
                SoundManager.instance.PlayOneShot(audio);
            }
        }
        else if (tag == "Top")
        {
            if (box)
            {
                hits++;
                sprite.sprite = on;
                if (hits >= 2)
                {
                    Instantiate(reward, transform.position + new Vector3(0, 3.5f, 0), Quaternion.identity);
                    sprite.gameObject.SetActive(false);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (tag == "Player1" || tag == "Player2")
        {
            if (lever)
            {
                if (turnedOff)
                {
                    sprite.sprite = on;
                    turnedOff = false;
                    turnedOn = true;
                }
                if (leverObject != null)
                    leverObject.SetActive(true);
                //else
                //{
                //    sprite.sprite = off;
                //    turnedOn = false;
                //    turnedOff = true;
                //}
            }
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
    //        //animator.enabled = false;
    //    sprite.sprite = off;
    //}
}

public enum ItemType
{
    Spring,
    Box,
    Button, 
    Flag,
    CheckPoint, 
    Lever
}