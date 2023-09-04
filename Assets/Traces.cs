using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traces : MonoBehaviour
{
    public void clearTrace()
    {
        Transform[] childList = GetComponentsInChildren<Transform>();
        if (childList != null)
        {
            for (int i = 0; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    Destroy(childList[i].gameObject);
            }
        }
    }
}
