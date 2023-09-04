using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    [Header("Commons")]
    public StageManager stageManager;
    public bool camStop;

    [Header("Cam Control")]
    [SerializeField] private Vector3 initPos;
    [SerializeField] private Vector3 movSpeed;

    private void Awake()
    {
        transform.position = initPos;
        camStop = false;
    }

    private void Update()
    {
        if(!camStop && transform.position.x > 0)
        {
            transform.position -= movSpeed;
        }
    }

    public void camMove_pos (Vector3 pos)
    {
        transform.position = pos;
    }
}
