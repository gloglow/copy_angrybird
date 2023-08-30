using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_s1 : MonoBehaviour
{
    public GameObject[] arr_gameObject;
    public Transform[] Clouds;
    public float cloudSpeed;
    private void Awake()
    {
        for (int i=0;  i<arr_gameObject.Length; i++) 
        { 
            arr_gameObject[i].SetActive(true);
        }
    }

    private void Update()
    {
        // Clouds move
        Clouds[1].position -= new Vector3(cloudSpeed, 0, 0);
        Clouds[0].position -= new Vector3(cloudSpeed, 0, 0);
        if (Clouds[0].position.x < -18.5)
        {
            Clouds[0].position = Clouds[1].position + new Vector3(18.5f, 0, 0);
        }
        else if (Clouds[1].position.x < -18.5)
        {
            Clouds[1].position = Clouds[0].position + new Vector3(18.5f, 0, 0);
        }
    }

    public void gotoNextScene()
    {
        SceneManager.LoadScene(0);
    }
}
