using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTop : MonoBehaviour
{
    public Enemy enemy;

    private void OnCollisionEnter2D(Collision2D collision)
    {      
        if (collision.gameObject.tag == "Player1" || collision.gameObject.tag == "Player2")
        {
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 400);
            Debug.Log("PLayer hit " + enemy.gameObject.name);
            enemy.hits++;
            enemy.health -= (enemy.maxHealth / enemy.hitsTillDeath);
            enemy.healthBar.SetHealth(enemy.health);
            if (enemy.hits >= enemy.hitsTillDeath)
                enemy.Die();
        }
    }



}
