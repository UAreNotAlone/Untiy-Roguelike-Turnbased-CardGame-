using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRange : MonoBehaviour 
{
    [Header("手设变量")]
    public int maxMoveLength = 2;
    
    public bool IsCheckValidMove(Transform selectedMapPoint,Player player)//检查移动是否合法
    {
        Debug.Log("begin check valid of move");
        if (selectedMapPoint == null) return false;//
        MapPoint Selectmappoint = selectedMapPoint.gameObject.GetComponent<MapPoint>();//在这里selectedMapPoint 是被鼠标选择的overlay
        //Debug.Log(selectedMapPoint.GetComponent<MapPoint>() == null);
        if (Selectmappoint == null)
        {
            Debug.Log("no MapPoint Script");
            return false;
        }
        else
        {
            Debug.Log("try to judge path valid");
            PathFinder _pathFinder = new PathFinder();
            List<OverlayTile> _path = _pathFinder.FindPath(player.onMapPoint.GetComponent<OverlayTile>(), selectedMapPoint.GetComponent<OverlayTile>());
            //Debug.Log(player.onMapPoint == null);
            if (_path.Count <= maxMoveLength)
            {
                 
                return true;
            }
            return false;
        }
        
    }
}
