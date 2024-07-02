using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door2 : MonoBehaviour
{
    public Item requiredItem;
    public int requiredNumber = 1;
    public Door2 otherDoor;
    public SpriteRenderer[] doorSprites = new SpriteRenderer[2];
    public Sprite[] openDoors = new Sprite[2];
    public bool exit = false;
    public bool visable = true, oneWay = false;

    [HideInInspector]
    public bool opened = false;
    PlayerUI ui;
    // Start is called before the first frame update
    void Start()
    {
        ui = PlayerUI.instance;
        if (!visable) gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            if ((oneWay & opened) || exit) return;
            if (opened) collision.transform.position = otherDoor.gameObject.transform.position;
            Debug.Log("Bumped into door");
            switch (requiredItem)
            {
                case Item.Keys:
                    if (ui.player == PlayerTag.Player1 && ui.gKeys >= requiredNumber)
                    {
                        ui.greenDoorOpen = true;
                        ui.gKeys -= requiredNumber;
                        ui.UpdateKeys(1);
                        OpenDoor(collision.gameObject);
                    }
                    else if (ui.player == PlayerTag.Player2 && ui.yKeys >= requiredNumber)
                    {
                        ui.yellowDoorOpen = true;
                        ui.yKeys -= requiredNumber;
                        ui.UpdateKeys(2);
                        OpenDoor(collision.gameObject);
                    }
                    break;
                case Item.Coins:
                    if (ui.score >= requiredNumber)
                    {
                        ui.score -= requiredNumber;
                        ui.ChangeCoins(ui.score);
                        OpenDoor(collision.gameObject);
                    }
                    break;
                case Item.Gems:
                    if (ui.player == PlayerTag.Player1 && ui.gGems >= requiredNumber)
                    {
                        ui.gGems -= requiredNumber;
                        OpenDoor(collision.gameObject);
                    }
                    else if (ui.player == PlayerTag.Player2 && ui.yGems >= requiredNumber)
                    {
                        ui.yGems -= requiredNumber;
                        OpenDoor(collision.gameObject);
                    }
                    break;
                case Item.Mojo:
                    if (ui.player == PlayerTag.Player1 && ui.greenMojo / ui.mojoPower >= requiredNumber)
                    {
                        for(int i = 0; i < requiredNumber; i++)
                          ui.ChangeMojo(1, -1);
                        OpenDoor(collision.gameObject);
                    }
                    else if (ui.player == PlayerTag.Player2 && ui.yellowMojo / ui.mojoPower >= requiredNumber)
                    {
                        for (int i = 0; i < requiredNumber; i++)
                            ui.ChangeMojo(2, -1);
                        OpenDoor(collision.gameObject);
                    }
                    break;
            }

        }
    }

    void OpenDoor(GameObject player)
    {
        player.transform.position = otherDoor.gameObject.transform.position;
        Unlock();
        otherDoor.Unlock();
        Debug.Log("Opening door");
    }

    void Unlock()
    {
        opened = true;
        for (int i = 0; i < 2; i++)
            doorSprites[i].sprite = openDoors[i];       
    }


}
