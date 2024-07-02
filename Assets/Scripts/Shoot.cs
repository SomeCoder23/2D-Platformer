using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject weapon;
    public Transform shootingPoint;
    public float timeBtwShots = 2f, timeToLive = 5f;
    public Vector2 direction = Vector2.up;
    public float power = 5f;

    private void Start()
    {
        InvokeRepeating("ShootObject", 1f, timeBtwShots);
    }

    void ShootObject()
    {
        GameObject obj = Instantiate(weapon, shootingPoint.position, Quaternion.identity);
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.AddForce(direction * power);
        Destroy(obj, timeToLive);
    }
}
