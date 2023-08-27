using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Enemy : MonoBehaviour
{
    int HP = 500;
    int score = 5000;

    public StageManager stageManager;
    public Rigidbody2D rigid;

    void Awake()
    {
        rigid=GetComponent<Rigidbody2D>();    
    }

    void Update()
    {
        
    }

    public void OnDamaged(int damage)
    {
        HP -= damage;
        Debug.Log(gameObject.name + damage);
        if (HP < 0)
        {
            Die();
        }
    }

    void Die()
    {
        stageManager.score += score;
        stageManager.enemyCnt--;
        gameObject.SetActive(false);
    }
}
