using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamController : MonoBehaviour
{
    public StageManager stageManager;

    public bool camStop;

    private void Awake()
    {
        transform.position = new Vector3(16, 0, -10);
        camStop = false;
    }

    private void Update()
    {
        if(!camStop && transform.position.x > 0)
        {
            transform.position -= new Vector3(0.01f, 0, 0);
        }
    }

    public void camMove_pos (Vector3 pos)
    {
        transform.position = pos;
    }
}
