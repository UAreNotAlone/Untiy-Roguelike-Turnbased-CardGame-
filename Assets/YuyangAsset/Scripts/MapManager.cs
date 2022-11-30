using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{

    private static MapManager _instance;


    public OverlayTile OverlayTilePrefab;
    public GameObject overlayContainer;
    public Dictionary<Vector2Int, OverlayTile> map; //  What for ?? -> Stores the upper layer of the tile//用以储存地图各点的坐标

    public static MapManager Instance
    {
        get { return _instance; }
    }

    private void Awake()
    {
        //  Make sure there is only one instance in a game scene.
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
        //  To activate all the cell with a overlay tile.
        var tileMap = gameObject.GetComponentInChildren<Tilemap>();
        Debug.Log(tileMap.name);
        BoundsInt bounds = tileMap.cellBounds;
        tileMap.CompressBounds();

        map = new Dictionary<Vector2Int, OverlayTile>();
        
        
        //  -Looping all the cells
        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.max.y; y > bounds.min.y; y--)
            {
                for (int x = bounds.max.x; x > bounds.min.x; x--)
                {
                    var tileLocation = new Vector3Int(x, y, z);
                    var tileKey = new Vector2Int(x, y);
                    if (tileMap.HasTile(tileLocation) && !map.ContainsKey(tileKey))
                    {
                        //Debug.Log(tileLocation);
                        var overlayTile = Instantiate(OverlayTilePrefab, overlayContainer.transform);
                        var cellWorldPosition = tileMap.GetCellCenterWorld(tileLocation);
                        //  Place it higher then the cell.
                        overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y,
                            cellWorldPosition.z + 1);
                       //Debug.Log(cellWorldPosition);
                        //  Align the sorting order.
                        overlayTile.GetComponent<SpriteRenderer>().sortingOrder =
                            tileMap.GetComponent<TilemapRenderer>().sortingOrder + 1;
                        overlayTile.gridLocation = tileLocation;
                        map.Add(tileKey, overlayTile);  //  Only record the upper layer tiles in the map.
                    }

                }
            }
        }
        
        
        
        
        
    }


}
