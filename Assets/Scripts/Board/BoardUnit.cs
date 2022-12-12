using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardUnit : MonoBehaviour
{
    [Header("PathFindingVariables")] 
    public int pathf_G;

    public int pathf_H;

    public int pathf_F
    {
        get { return pathf_G + pathf_H; }
    }

    public bool isBlocked = false;
    public BoardUnit previousBoardUnit;
    public Vector3Int gridLocation;
    public Vector2Int grid2DLocation
    {
        get { return new Vector2Int(gridLocation.x, gridLocation.y); }
    }
    private void Update()
    {
        //  TODO: Trigger this when it is player's turn?.
        if (Input.GetMouseButtonDown(0))
        {
            hideBoardUnit();
        }
    }

    
    //  Change the Transparency.
    public void showBoardUnit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }

    public void hideBoardUnit()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    public void OnSelectedByCard()
    {
        transform.Find("Cursor").GetComponent<SpriteRenderer>().enabled = true;
    }

    public void OnUnselectedByCard()
    {
        transform.Find("Cursor").GetComponent<SpriteRenderer>().enabled = false;
    }
}
