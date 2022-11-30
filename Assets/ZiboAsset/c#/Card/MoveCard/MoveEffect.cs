using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : MonoBehaviour, IEffect
{
    private Player player;
    private PathFinder _pathFinder;
    public float speed = 20f;


    public void Start()
    {
        _pathFinder = new PathFinder();
        player = gameObject.GetComponent<CardClass>().holdPlayer;
    }
    public void OnEnable()
    {


    }
    // Start is called before the first frame update
    public void CauseEffect(List<Transform> targetList)//target 是Overlay的transform//在移动中targetList只包含一个元素
    {
        Transform target = targetList[0];
        OverlayTile targetOlt = target.GetComponent<OverlayTile>();
        if (targetOlt == null)
        {
            Debug.Log("Target Olt is empty");
        }
        List<OverlayTile> _path = _pathFinder.FindPath(player.onMapPoint.GetComponent<OverlayTile>(), targetOlt);//不包括自身所在的点

        MovePlayer movePlayer = GameObject.Find("MovePlayer").transform.GetComponent<MovePlayer>();

        Debug.Log("Begin to move");
        player.onMapPoint = target.GetComponent<MapPoint>();
        movePlayer.MoveAlongPath(player, _path,speed);
    }

}