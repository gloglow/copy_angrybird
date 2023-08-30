using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public bool isCurrent;
    public bool readyFlag = false;

    // Range of Charge
    public float maxMouse_x;
    public float maxMouse_y;
    public float maxMag;

    public float shootPower;
    public float damageAdj;
    public float damageWeighting;
    

    public GameManager gameManager;
    public StageManager stageManager;
    public SoundManager soundManager;
    Animator animator;
    Rigidbody2D rigid;

    Vector3 crtPos;
    Vector3 pastPos;

    public bool onGround = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isReady", true);
    }

    void Update()
    {
        if(isCurrent && animator.GetBool("isReady"))
        {
            animator.SetBool("isReady", false);
        }

        // Check Dragged or not
        if (readyFlag == true)
        {
            readyToShoot();

            // Shoot
            if (Input.GetMouseButtonUp(0))
            {
                // Change Layer -> Layer 9 : cannot be clicked
                gameObject.layer = 9;
                Shoot();
                readyFlag = false;
            }
        }

        // Rotate During Flying
        if (gameObject.layer == 9 && !onGround)
        {
            crtPos = transform.position;
            if (pastPos == new Vector3(0,0,0))
            {
                pastPos = transform.position;
            }
            else if(pastPos!=crtPos)
            {
                Vector3 vec=crtPos - pastPos;
                transform.right=vec.normalized;
                pastPos = crtPos;
            }
        }

    }

    private void FixedUpdate()
    {
        // Check On ground or not
        if (gameObject.layer == 9 && !onGround)
        {
            Debug.DrawRay(rigid.position, Vector3.down, Color.yellow);
            RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, Vector3.down, 1, LayerMask.GetMask("Floor"));
            if (rayHit.collider != null)
            {
                onGround = true;
            }
        }

        // Die = After shooted + low velocity or Fall
        if ((gameObject.layer == 9 && rigid.velocity.magnitude <= 0.03f) || transform.position.y < -6)
        {
            Die();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        onGround = true;

        // Calculate Damage
        int damage = ((int)(rigid.velocity.magnitude / damageAdj * rigid.mass * damageWeighting)) / 10 * 10;

        // Collision with Enemy or Obstacle
        if (collision.gameObject.layer == 7 || collision.gameObject.layer == 8)
        {
            OnAttack(collision.transform, collision.gameObject.layer, damage);
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

    public void readyToShoot()
    {
        // Follow Mouse
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        // Range of mouse when charging
        if(Mathf.Abs(stageManager.startPos.x - mousePos.x) > maxMouse_x)
        {
            if (Mathf.Abs(stageManager.startPos.y - mousePos.y) > maxMouse_y)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 0);
                return;
            }
            transform.position = new Vector3(transform.position.x, mousePos.y, 0);
            return;
        }
        if(Mathf.Abs(stageManager.startPos.y - mousePos.y) > maxMouse_y)
        {
            transform.position = new Vector3(mousePos.x, transform.position.y, 0);
            return;
        }

        transform.position = new Vector3(mousePos.x, mousePos.y, 0);
    }
    void Shoot()
    {
        stageManager.traceClear();

        // Shooting sound
        soundManager.ShootingSound();

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

    void Die()
    {
        gameObject.SetActive(false);
        stageManager.nextCharacter();
    }
}
