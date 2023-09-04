using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTrace : MonoBehaviour
{
    [Header("Commons")]
    public GameObject traces;

    private void Awake()
    {
        gameObject.transform.SetParent(null);
        traces = GameObject.Find("Traces");
        gameObject.transform.SetParent(traces.transform);
    }
}
