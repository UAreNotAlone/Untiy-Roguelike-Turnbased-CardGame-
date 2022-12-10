using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOverlayContainer : MonoBehaviour
{
    public List<OverlayTile> oldMapPointList= new List<OverlayTile>();
    public  List<OverlayTile> MapPointList;
    public  GameObject overLayContainer;
    private PathFinder _pathFinder;
    // Start is called before the first frame update
    private void Start()
    {
        _pathFinder = new PathFinder();
        overLayContainer = GameObject.Find("OverlayContainer");
        if (overLayContainer != null)
        {
            InvokeRepeating(nameof(ConitnueInitialOverLays), 0f, 1f);
        }
        else
        {
            Debug.Log("fail to find overLayContainer");
        }
    }
    public void ConitnueInitialOverLays()//需要生成
    {
        MapPointList =new List<OverlayTile>(overLayContainer.GetComponentsInChildren<OverlayTile>());

        bool changed = CheckIfMapChanged();
        if (!changed) return;
        AddMapScript();
        //Debug.Log("Do assign neighbor*1 ");
        ChangeLayer();
        InitialAroundMapPoints();

        MatchDirections();
        oldMapPointList = MapPointList;
    }
    public void ChangeLayer()
    {
        foreach (OverlayTile olt in MapPointList)
        {
            olt.gameObject.layer = LayerMask.NameToLayer("Overlay");

        }
    }
    public bool CheckIfMapChanged()
    {
        foreach(OverlayTile olt in MapPointList)
        {
            if (!oldMapPointList.Contains(olt))
            {
                return true;
            }
            
        }
        return false;
    }
    public void AddMapScript()
    {
        foreach (OverlayTile point in MapPointList)
        {
            if (point.gameObject.GetComponent<MapPoint>() == null)
            {
                point.gameObject.AddComponent<MapPoint>();
            }
        }
    }

    public void InitialAroundMapPoints()
    {
        foreach (OverlayTile point in MapPointList)
        {
            MapPoint mapPoint = point.GetComponent<MapPoint>();
            List<OverlayTile> neighbor = _pathFinder.GetNeighbourTiles(point);//////

            foreach (OverlayTile olt in neighbor)
            {

                mapPoint.aroundMapPoints.Add(olt.gameObject.GetComponent<MapPoint>());
            }
        }
    }
    public void MatchDirections()
    {
        foreach (OverlayTile point in MapPointList)
        {
            List<Vector2> Directions = new List<Vector2>();
            Vector2 Added1 = new Vector2(1, 1);
            Directions.Add(Added1);
            Vector2 Added2 = new Vector2(1, -1);
            Directions.Add(Added2);
            Vector2 Added3 = new Vector2(-1, 1);
            Directions.Add(Added3);
            Vector2 Added4 = new Vector2(-1, -1);
            Directions.Add(Added4);
            point.GetComponent<MapPoint>().MarkMapPoint(point.GetComponent<MapPoint>().aroundMapPoints, Directions);
        }
    }
}
