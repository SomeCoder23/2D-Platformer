using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCam : MonoBehaviour
{
    public GameObject player;

    private void FixedUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, -10f);
    }
}
