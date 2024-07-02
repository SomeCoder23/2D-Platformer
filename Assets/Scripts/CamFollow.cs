using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform player;
    public bool horizontal = true;
    public int size = 33;
    public Transform topBounds, bottomBounds;
    float posY, startY, posX;
    Vector3 pos;
    private void Start()
    {
        posY = transform.position.y;
        posX = transform.position.x;
        startY = posY;
    }
    void FixedUpdate()
    {
        if (horizontal)
        {
            if (player.position.y > topBounds.position.y || player.position.y < bottomBounds.position.y)
                posY = player.transform.position.y;
            else posY = transform.position.y;

        pos = new Vector3(player.position.x + 33, posY, -10);
        }
        else
        {
            pos = new Vector3(posX, player.position.y + 10, -10);
        }

        //else if(player.position.y < yBounds.y)
        ////    posY = player.transform.position.y;
        transform.position = pos;
        //transform.LookAt(player);
    }
}
