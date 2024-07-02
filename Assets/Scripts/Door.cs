using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool bonus = true;
    public Item requiredItem;
    public int requiredNumber = 1;
    public float time = 30;
    public Transform otherDoor;
    public SpriteRenderer[] doorSprites = new SpriteRenderer[2];
    public Sprite[] openDoors = new Sprite[2];
    public Camera cam, otherCam;
    public GameObject separator;
    public Door otherPlayerDoor;
    public GameObject otherPlayer;
    public Timer clock;

    PlayerUI ui;
    bool opened = false, bLevelStarted = false, openedOther = false;
    GameObject player;
    int timer;

    void Start()
    {
        timer = int.Parse(time.ToString());
        Debug.Log("Timer = " + timer + " seconds");
        ui = PlayerUI.instance;
        if (ui.singlePlayer)
            openedOther = true;

        if (!bonus)
        {
            cam = Camera.main;
            otherCam = Camera.main;
        }

    }

    void Update()
    {
        if (bLevelStarted && bonus)
        {
            if (time >= 0)
            {
                /*Debug.Log(time + " time left");*/
                time -= Time.deltaTime;
            }
            else
            {
                if (clock != null)
                {
                    clock.gameObject.SetActive(false);
                    CancelInvoke("Timer");
                }
                bLevelStarted = false;
                player.transform.position = transform.position;
                if (!ui.singlePlayer)
                {
                    cam.gameObject.SetActive(true);
                    otherCam.gameObject.SetActive(false);
                    separator.SetActive(false);
                    for (int i = 0; i < ui.healthBars.Length; i++)
                        ui.healthBars[i].gameObject.SetActive(true);
                }
                for (int i = 0; i < ui.greenKeys.Length; i++)
                    ui.greenKeys[i].gameObject.SetActive(true);

                for (int i = 0; i < ui.yellowKeys.Length; i++)
                    ui.yellowKeys[i].gameObject.SetActive(true);
                for (int i = 0; i < ui.gemText.Length; i++)
                    ui.gemText[i].gameObject.transform.parent.gameObject.SetActive(true);
            }
          

        }
    }
    

    

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (!opened)
    //    {
    //        if (collision.gameObject.tag == "Player1")
    //        {
    //            if (ui.gKeys >= requiredKeys)
    //                OpenDoor(collision.gameObject);
    //        }
    //        else if (collision.gameObject.tag == "Player2")
    //        {
    //            if (ui.yKeys >= requiredKeys)
    //                OpenDoor(collision.gameObject);
    //        }
    //    }
    //}

    public void OpenDoor(GameObject p)
    {
            if (opened) return;
            else if (ui.greenDoorOpen && ui.yellowDoorOpen)
            {
                if (!openedOther)
                {
                    openedOther = true;
                    otherPlayerDoor.openedOther = true;
                    otherPlayerDoor.OpenDoor(otherPlayer);
                }
                Debug.Log("opening door");
                if (clock != null)
                {
                    clock.gameObject.SetActive(true);
                    InvokeRepeating("Timer", 0f, 1f);
                }
                player = p;
                player.transform.position = otherDoor.position;
                for (int i = 0; i < 2; i++)
                {
                    doorSprites[i].sprite = openDoors[i];
                }
                opened = true;
                bLevelStarted = true;
                if (!ui.singlePlayer)
                {
                    cam.gameObject.SetActive(false);
                    otherCam.gameObject.SetActive(true);
                    if (bonus)
                        for (int i = 0; i < ui.healthBars.Length; i++)
                            ui.healthBars[i].gameObject.SetActive(false);
                    separator.SetActive(true);
                }
                for (int i = 0; i < ui.greenKeys.Length; i++)
                    ui.greenKeys[i].gameObject.SetActive(false);

                for (int i = 0; i < ui.yellowKeys.Length; i++)
                    ui.yellowKeys[i].gameObject.SetActive(false);
                for (int i = 0; i < ui.gemText.Length; i++)
                    ui.gemText[i].gameObject.transform.parent.gameObject.SetActive(false);
            }
        
    }


    void Timer()
    {
        clock.CountDown(timer);
        timer--;
        Debug.Log("Time left: " + timer + " seconds");
    }
}



public enum Item
{   
    Keys,
    Coins,
    Mojo,
    Gems
}
