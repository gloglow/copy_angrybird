using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public Character prefab_Character;
    public Enemy prefab_Enemy;
    public Obstacle prefab_Obstacle;
    public GameManager gameManager;
    public Text resultText;
    public Text scoreText;
    public Canvas resultUI;
    public Canvas onplayUI;
    public Canvas pauseUI;
    public Canvas retryUI;

    public List<Character> characterList;
    public int enemyCnt;

    Vector3 mousepos;
    public Vector3 startPos;
    public int score;

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
            
            // Ray for Character
            RaycastHit2D rayHit = Physics2D.Raycast(pos, Vector3.zero, 1, LayerMask.GetMask("Character"));
            if (rayHit.collider!=null)
            {
                characterList[0].readyFlag = true;
            }
        }

        if (enemyCnt == 0)
        {
            StageClear();
        }
    }

    public void nextCharacter()
    {
        if (characterList.Count == 1)
        {
            StageFailed();
        }
        else
        {
            characterList.RemoveAt(0);
        }
    }

    void StageClear()
    {
        resultUI.gameObject.SetActive(true);
        resultText.text = "LEVEL CLEARED!";
        Time.timeScale = 0f;
    }

    void StageFailed()
    {
        resultUI.gameObject.SetActive(true);
        resultText.text = "LEVEL FAILED!";
        Time.timeScale = 0f;
    }

    public void Pause()
    {
        onplayUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Continue()
    {
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
