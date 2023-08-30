using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public int HP = 500;
    int score = 500;

    public float damageAdj;
    public float damageWeighting;

    public StageManager stageManager;
    public SoundManager soundManager;
    public EffectManager effectManager;
    public GameObject prefab_enemyText;
    public Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (stageManager.score > 0)
        {
            int damage = ((int)(rigid.velocity.magnitude / damageAdj * rigid.mass * damageWeighting)) / 10 * 10;

            // Collision with Enemy or Obstacle
            if (damage != 0 && (collision.gameObject.layer == 7 || collision.gameObject.layer == 8))
            {
                OnAttack(collision.transform, collision.gameObject.layer, damage);
            }
        }   
    }

    void OnAttack(Transform obj, int layerNum, int damage)
    {
        // Collision with Enemy
        if (layerNum == 7)
        {
            Enemy enemy = obj.GetComponent<Enemy>();
            enemy.OnDamaged(damage);
        }
        // Coliision with Obstacle
        else
        {
            Obstacle obstacle = obj.GetComponent<Obstacle>();
            obstacle.OnDamaged(damage);
        }
    }

    public void OnDamaged(int damage)
    {
        // Damage Sound
        soundManager.damagedSound();

        // Get Damage
        HP -= damage;

        // Update Score
        stageManager.score += damage;

        // Die
        if (HP < 0)
        {
            Die();
        }

        // Show Damage Effect
        effectManager.makeText(transform.position, damage);
    }

    void Die()
    {
        // Destruction sound
        soundManager.destSound();

        effectManager.makeEffect(transform.position);

        // No collision
        gameObject.layer = 10;

        // Update Score
        stageManager.score += score;

        // Destroy
        Destroy(gameObject);
    }

}
