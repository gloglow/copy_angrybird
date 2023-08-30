using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public CamController cam;
    public Character prefab_Character;
    public Enemy prefab_Enemy;
    public Obstacle prefab_Obstacle;
    public GameManager gameManager;
    public SoundManager soundManager;
    public CharacterTrace prefab_characterTrace;
    public Text resultText;
    public Text scoreText;
    public Text scoreText_result;
    public Text highscoreText_result;
    public Image[] stars_crt;
    public Image[] stars_high;
    public Canvas resultUI;
    public Canvas onplayUI;
    public Canvas pauseUI;
    public Canvas retryUI;
    public Transform[] Clouds;

    public List<Character> characterList;
    public int enemyCnt;
    bool endFlag=false;
    public List<CharacterTrace> traceList;

    Vector3 mousepos;
    public Vector3 startPos;
    public int score;
    public float cloudSpeed;

    private void Awake()
    {
        score = 0;

        resultUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(false);
        retryUI.gameObject.SetActive(false);

        Time.timeScale = 1f;
    }

    void Update()
    {
        // score text
        scoreText.text = "Score : " + score;

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
            if (move.x > 22)
            {
                move.x = 22;
            }
            else if(move.x < -1.4f)
            {
                move.x = -1.4f;
            }
            cam.camMove_pos(move);
            cam.camStop=true;
        }

        // Draw trace during Flying
        if (characterList[0].gameObject.layer == 9 && !characterList[0].onGround) 
        {
            DrawTrace();
        }

        // Level Clear 
        if (enemyCnt == 0 && endFlag == false)
        {
            score += (characterList.Count - 1) * 10000;
            endFlag = true;
            Invoke("LevelClear", 3f);
        }


        // Camera during flying
        if (characterList[0].transform.position.x > 0)
        {
            float xCamPos = characterList[0].transform.position.x;
            if(xCamPos >= 21f)
            {
                xCamPos = 21f;
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

    void DrawTrace()
    {
        CharacterTrace characterTrace = Instantiate(prefab_characterTrace, transform);
        characterTrace.transform.position = characterList[0].transform.position;
        traceList.Add(characterTrace);
    }

    public void traceClear()
    {
        for (int i = 0; i < traceList.Count; i++) 
        {
            Destroy(traceList[i].gameObject);
        }
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

    void LevelClear()
    {
        // Result UI
        resultUI.gameObject.SetActive(true);
        resultText.text = "LEVEL CLEARED!";
        //score += (characterList.Count-1) *10000;
        scoreText_result.text = "" + score;

        // Star
        stars_crt[1].gameObject.SetActive(false);
        if (score >= 15000)
        {
            stars_crt[3].gameObject.SetActive(false);
            if (score >= 20000)
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
        resultUI.gameObject.SetActive(true);
        resultText.text = "LEVEL FAILED!";
        scoreText_result.text = "" + score;

        Time.timeScale = 0f;
    }

    public void Pause()
    {
        // Pause Button
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
        onplayUI.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    public void Retry()
    {
        // Retry Button
        onplayUI.gameObject.SetActive(false);
        retryUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void gotoScene2()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void goRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
