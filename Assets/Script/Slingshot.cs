using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public Transform transPoint1;
    public Transform transPoint2;
    public StageManager stageManager;

    LineRenderer lineRenderer;

    bool shootflag;

    private void Awake()
    {
        lineRenderer=GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.material.SetColor("_Color", Color.black);
    }

    private void Update()
    {
        if (stageManager.characterList[0].readyFlag)
        {
            if (lineRenderer.positionCount < 3)
            {
                lineRenderer.positionCount = 3;
            }
            Vector3 newPos = stageManager.characterList[0].transform.position;
            lineRenderer.SetPosition(1, newPos);
            shootflag = true;
        }
        if (transPoint1 && transPoint2)
        {
            lineRenderer.SetPosition(0, transPoint1.position);
            lineRenderer.SetPosition(lineRenderer.positionCount-1, transPoint2.position);
        }

        if (shootflag==true && stageManager.characterList[0].gameObject.layer == 9)
        {
            lineRenderer.positionCount--;
            shootflag=false;
        }
    }
}
