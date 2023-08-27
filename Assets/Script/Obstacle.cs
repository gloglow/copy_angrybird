using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    int HP = 500;
    int score = 500;

    public StageManager stageManager;
    public Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }
    public void OnDamaged(int damage)
    {
        HP -= damage;
        stageManager.score += damage;

        Debug.Log(gameObject.name + damage);

        if (HP < 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        stageManager.score += score;
        gameObject.SetActive(false);
    }
}
