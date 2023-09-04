using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header ("Prefab")]
    public Character prefab_Character;
    public Enemy prefab_Enemy;
    public Obstacle prefab_Obstacle;

    [Header ("Commons")]
    public CamController cam;
    public GameManager gameManager;
    public SoundManager soundManager;
    public List<Character> characterList;

    [Header ("UI")]
    [SerializeField] private Text resultText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text scoreText_result;
    [SerializeField] private Image[] stars_crt;
    [SerializeField] private Canvas panel;
    [SerializeField] private Canvas resultUI;
    [SerializeField] private Canvas onplayUI;
    [SerializeField] private Canvas pauseUI;
    [SerializeField] private Canvas retryUI;

    [Header ("Background")]
    [SerializeField] private Transform[] Clouds;
    [SerializeField] private float cloudSpeed;

    [Header("CamControl")]
    [SerializeField] private float camRangeRight;
    [SerializeField] private float camRangeLeft;

    [Header ("Variable for play")]
    public int enemyCnt;
    [SerializeField] private bool endFlag =false;
    [SerializeField] private Vector3 mousepos;
    public Vector3 startPos;
    public int score;
    [SerializeField] private int star2Cdtion;
    [SerializeField] private int star3Cdtion;
    private int lifeBonus = 10000;


    private void Awake()
    {
        score = 0;
        scoreText.text = "Score : " + score;

        panel.gameObject.SetActive(false);
        resultUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(false);
        retryUI.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        // Current Character Update
        if (characterList[0].isCurrent == false)
        {
            characterList[0].isCurrent = true;
            characterList[0].gameObject.layer = 6;
        }

        // Waitng for Drag
        if(!characterList[0].readyFlag && characterList[0].gameObject.layer!=9)
        {
            characterList[0].onStage();
        }

        // Check Mouse Click
        if (Input.GetMouseButtonDown(0))
        {
            mousepos = Input.mousePosition;
            Vector3 pos = Camera.main.ScreenToWorldPoint(mousepos);
            
            // Click and drag Character
            RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector3.zero, 1, LayerMask.GetMask("Character"));
            if (rayHit.collider!=null)
            {
                // Bow string sound
                soundManager.bowChargeSound();
                characterList[0].readyFlag = true;
            }
        }

        // Click and drag for move Camera
        if (Input.GetMouseButton(0) && characterList[0].readyFlag != true)
        {
            Vector3 move = new Vector3((mousepos.x-Input.mousePosition.x)/20, 0, -10);

            // Range of Camera position
            if (move.x > camRangeRight)
            {
                move.x = camRangeRight;
            }
            else if(move.x < camRangeLeft)
            {
                move.x = camRangeLeft;
            }
            cam.camMove_pos(move);
            cam.camStop=true;
        }

        // Level Clear 
        if (enemyCnt == 0 && endFlag == false)
        {
            score += (characterList.Count - 1) * lifeBonus;
            endFlag = true;
            Invoke("LevelClear", 3f);
        }


        // Camera during flying
        if (characterList[0].transform.position.x > 0)
        {
            float xCamPos = characterList[0].transform.position.x;
            if(xCamPos >= camRangeRight)
            {
                xCamPos = camRangeRight;
            }
            cam.camMove_pos(new Vector3(xCamPos, 0, -10));
        }

        // Clouds move
        for (int i = 0; i<Clouds.Length; i++)
        {
            Clouds[i].position -= new Vector3(cloudSpeed, 0, 0);
            if (Clouds[i].position.x < -18.5)
            {
                int j = (i != Clouds.Length-1) ? i + 1 : 0;
                Clouds[i].position = Clouds[j].position + new Vector3(54f, 0, 0);
            }
        }
    }

    public void ScorePlus(int plusScore)
    {
        score += plusScore;
        scoreText.text = "Score : " + score;
    }

    public void nextCharacter()
    {
        // Level Failed
        if (characterList.Count == 1)
        {
            Invoke("LevelFailed", 2f);
        }

        // Next Character
        else
        {
            characterList.RemoveAt(0);
            cam.camStop = false;
        }
    }

    void ShowPanel()
    {
        panel.gameObject.SetActive(true);
    }

    void ClosePanel()
    {
        panel.gameObject.SetActive(false);
    }

    void LevelClear()
    {
        // Result UI
        ShowPanel();
        resultUI.gameObject.SetActive(true);
        resultText.text = "LEVEL CLEARED!";
        scoreText_result.text = "" + score;

        // Star
        stars_crt[1].gameObject.SetActive(false);
        if (score >= star2Cdtion)
        {
            stars_crt[3].gameObject.SetActive(false);
            if (score >= star3Cdtion)
            {
                stars_crt[5].gameObject.SetActive(false);
            }
        }

        Time.timeScale = 0f;
        endFlag = true;
    }

    void LevelFailed()
    {
        // Result UI
        ShowPanel();
        resultUI.gameObject.SetActive(true);
        resultText.text = "LEVEL FAILED!";
        scoreText_result.text = "" + score;

        Time.timeScale = 0f;
    }

    public void Pause()
    {
        // Pause Button
        ShowPanel();
        onplayUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue()
    {
        // Continue Button
        if (pauseUI.gameObject.activeInHierarchy)
        {
            pauseUI.gameObject.SetActive(false);
        }
        else
        {
            retryUI.gameObject.SetActive(false);
        }
        ClosePanel();
        onplayUI.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Retry()
    {
        ShowPanel();
        // Retry Button
        onplayUI.gameObject.SetActive(false);
        retryUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void gotoSceneTitle()
    {
        SceneManager.LoadScene("GameTitle");
    }

    public void goRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
