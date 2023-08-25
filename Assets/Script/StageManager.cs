using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public Character prefab_Character;
    public Enemy prefab_Enemy;
    public Obstacle prefab_Obstacle;
    public GameManager gameManager;

    public Character[] arr_Character;
    public Enemy[] arr_Enemy;

    Vector3 mousepos;
    public Vector3 startPos;

    void Update()
    {
        // Current Character Update
        if (arr_Character[0].isCurrent == false)
        {
            arr_Character[0].isCurrent = true;
        }

        // Waitng for Drag
        if(!arr_Character[0].readyFlag && arr_Character[0].gameObject.layer!=9)
        {
            arr_Character[0].onStage();
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
                arr_Character[0].readyFlag = true;
            }
        }
    }
}
