using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    //  Costs for A* pathfinding algorithm
    [Header("PathFinding")]
    public int pathf_G;
    public int pathf_H;
    public int pathf_F
    {
        get { return pathf_G + pathf_H; }
    }
    public bool isBlocked = true;
    public OverlayTile previousTile;
    public Vector3Int gridLocation ;
    
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HideTail();
        }
    }


    public void showTail()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
    }
    
    public void HideTail(){
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }
}
