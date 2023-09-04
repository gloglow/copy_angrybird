using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    private bool flag = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if(!flag)
        {
            transform.localScale += new Vector3(0.008f, 0.008f, 0.008f);
        }
        else 
        {
            transform.localScale -= new Vector3(0.005f, 0.008f, 0.008f);
            
        }

        if (transform.localScale.x >= 0.3f)
        {
            flag= true;
        }

        if (transform.localScale.x <= 0)
        {
            Destroy(gameObject);
        }
    }
}
