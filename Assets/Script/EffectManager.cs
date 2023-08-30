using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    public Effects prefab_effect;
    public GameObject prefab_enemyText;

    public void makeText(Vector3 pos, int damage)
    {
        GameObject obj = Instantiate(prefab_enemyText);
        obj.transform.position = pos;
        TextMeshPro textMeshPro = obj.GetComponentInChildren<TextMeshPro>();
        textMeshPro.text = damage.ToString();
    }

    public void makeEffect(Vector3 pos)
    {
        Effects effect = Instantiate(prefab_effect);
        effect.transform.position = pos;
    }
}
