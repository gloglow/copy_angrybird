using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_s1 : MonoBehaviour
{
    public GameObject[] arr_gameObject;
    private void Awake()
    {
        for (int i=0;  i<arr_gameObject.Length; i++) 
        { 
            arr_gameObject[i].SetActive(true);
        }
    }
    public void gotoNextScene()
    {
        SceneManager.LoadScene(0);
    }
}
