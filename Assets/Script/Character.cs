using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isCurrent;
    public bool readyFlag = false;

    // Range of Charge
    float maxMouse_x = -9f; 
    float maxMouse_y = -2f;
    float maxMag = 3f;

    float shootPower = 9f;

    public GameManager gameManager;
    public StageManager stageManager;
    Animator animator;

    Rigidbody2D rigid;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator= GetComponent<Animator>();
        animator.SetBool("isReady", false);
    }

    void Update()
    {
        // Check Dragged or not
        if (readyFlag==true)
        {
            readyToShoot();

            // Shoot
            if(Input.GetMouseButtonUp(0))
            {
                // Change Layer -> Layer 9 : cannot be clicked
                this.gameObject.layer = 9;
                Shoot();
                readyFlag = false;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Collision with Enemy
        if(collision.gameObject.layer == 7)
        {

        }

        // Collision with Obstacle
        if(collision.gameObject.layer == 8)
        {

        }
    }

    public void readyToShoot()
    {
        // Stop animation during charging
        animator.SetBool("isReady", true);

        // Follow Mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePos.x < maxMouse_x)
        {
            mousePos.x = maxMouse_x;
        }
        if(mousePos.y < maxMouse_y)
        {
            mousePos.y = maxMouse_y;
        }

        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
    void Shoot()
    {
        animator.SetBool("isReady", false);
        rigid.gravityScale = 1;

        // Set Shooting Direction and Power
        Vector3 vec = stageManager.startPos-transform.position;
        Vector3 dirvec = vec.normalized;
        float magvec = vec.magnitude;
        if(magvec > maxMag)
        {
            magvec = maxMag;
        }
        rigid.velocity= dirvec*magvec*shootPower;
    }

    public void onStage() // ready to fly
    {
        transform.position = stageManager.startPos;
        rigid.gravityScale = 0;
    }
}
