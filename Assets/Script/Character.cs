using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header ("Commons")]
    public StageManager stageManager;
    public SoundManager soundManager;
    public Traces traces;
    private Animator animator;
    private Rigidbody2D rigid;

    [Header("Prefab")]
    public CharacterTrace prafab_characterTrace;

    [Header ("Charge Property")]
    [SerializeField] private float maxMouse_x;
    [SerializeField] private float maxMouse_y;
    [SerializeField] private float maxMag;

    [Header ("Shooting Property")]
    [SerializeField] private float shootPower;
    [SerializeField] private float damageAdj;
    [SerializeField] private float damageWeighting;

    [Header ("Vector for calculate")]
    private Vector3 crtPos;
    private Vector3 pastPos;

    [Header ("Managing")]
    public bool isCurrent;
    public bool readyFlag = false;
    public bool onGround = false;
    float minVelocity = 0.03f;
    float fallY = -6f;

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

        // Draw Trace && Rotate During Flying
        if (gameObject.layer == 9 && !onGround)
        {
            DrawTrace();
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
        if ((gameObject.layer == 9 && rigid.velocity.magnitude <= minVelocity) || transform.position.y < fallY)
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

    void DrawTrace()
    {
        CharacterTrace crtTrace = Instantiate(prafab_characterTrace);
        crtTrace.transform.position = transform.position;
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
        float distanceX = mousePos.x - stageManager.startPos.x;
        distanceX=Mathf.Clamp(distanceX, -maxMouse_x, maxMouse_x);

        float distanceY = mousePos.y - stageManager.startPos.y;
        distanceY = Mathf.Clamp(distanceY, -maxMouse_y, maxMouse_y);

        transform.position = stageManager.startPos + new Vector3(distanceX, distanceY, 0);
    }
    void Shoot()
    {
        // Erase trace
        traces.clearTrace();

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
        onGround = false;
        rigid.gravityScale = 0;
    }

    void Die()
    {
        gameObject.SetActive(false);
        stageManager.nextCharacter();
    }
}
