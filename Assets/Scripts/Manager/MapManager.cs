using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;
using Unity.VisualScripting;

public class MapManager : MonoBehaviour
{
    
    //Singleton
    public static MapManager Instance { get; private set; }

    private GameObject _fightBoard_obj;
    private Tilemap _fightBoard_tilemap;
    //private BoardUnit _boardUnit_prefab;
    
    public Dictionary<Vector2Int, BoardUnit> fightBoard_dict;
    public GameObject boardUnitContainer;

    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
            /*
            GameObject temp_obj = Resources.Load("TileMaps/BoardUnit") as GameObject;
            _boardUnit_prefab = temp_obj.GetComponent<BoardUnit>();
            */
        }
    }

    private void LoadBoardIntoScene()
    {
        _fightBoard_obj = Instantiate(Resources.Load("TileMaps/MainFightBoard")) as GameObject;
    }


    public void InitMap()
    {
        //  Load the Map to Scene
        LoadBoardIntoScene();
        //Debug.Log(_fightBoard_obj.name);
        if(_fightBoard_obj == null)
        {
            Debug.Log("[MapManager]: Map failes to be loaded into the scene");
            return;
        }
    
        //  Fetch the tilemap component in it.
        _fightBoard_tilemap = _fightBoard_obj.GetComponentInChildren<Tilemap>();
        if (_fightBoard_tilemap == null)
        {
            Debug.Log("[MapManager]: No tileMap found in the child");
            return;
        }
        Debug.Log(_fightBoard_tilemap.name);

        BoundsInt bounds = _fightBoard_tilemap.cellBounds;
        _fightBoard_tilemap.CompressBounds();
        fightBoard_dict = new Dictionary<Vector2Int, BoardUnit>();
        
        
        //  Looping and init all the cells
        for (int z = bounds.max.z; z >= bounds.min.z; z--)
        {
            for (int y = bounds.max.y; y > bounds.min.y; y--)
            {
                for (int x = bounds.max.x; x > bounds.min.x; x--)
                {
                    Vector3Int boardUnitLocation = new Vector3Int(x, y, z);
                    Vector2Int boardUnitKey = new Vector2Int(x, y);
                    if (_fightBoard_tilemap.HasTile(boardUnitLocation) && !fightBoard_dict.ContainsKey(boardUnitKey))
                    {
                        //  Instantiate the board unit and add the scripts into it.
                        GameObject boardUnit_obj = Instantiate(Resources.Load("TileMaps/BoardUnit"), boardUnitContainer.transform) as GameObject;
                        BoardUnit boardUnit = boardUnit_obj.AddComponent<BoardUnit>();
                        //  Position it on the map board unit.
                        Vector3 boardWorldPosition = _fightBoard_tilemap.GetCellCenterWorld(boardUnitLocation);
                        boardUnit.transform.position = new Vector3(boardWorldPosition.x, boardWorldPosition.y,
                            boardWorldPosition.z + 1);
                        
                        //  Aligh the Sorting order
                        boardUnit.GetComponent<SpriteRenderer>().sortingOrder =
                            _fightBoard_tilemap.GetComponent<TilemapRenderer>().sortingOrder + 1;

                        boardUnit.gridLocation = boardUnitLocation;
                        fightBoard_dict.Add(boardUnitKey, boardUnit);
                        
                    }
                }
            }
        }

        return;
    }
    
     //  The path Finder.
        public List<BoardUnit> FindPath(BoardUnit start, BoardUnit end)
        {
            List<BoardUnit> openList = new List<BoardUnit>();
            List<BoardUnit> closeList = new List<BoardUnit>();
            Debug.Log("From " + start.gridLocation + " t o" + end.gridLocation);
            openList.Add(start);
            Debug.Log("[FindPath]: PathFinder starts to work.");
            // Find the shortest path
            while (openList.Count > 0)
            {
                Debug.Log("[FindPath]: PathFinder in the while loop.");
                // Get the node with the smallest F cost.
                BoardUnit currentOverlayTile = openList.OrderBy(x => x.pathf_F).First();
                openList.Remove(currentOverlayTile);
                closeList.Add(currentOverlayTile);
                if (currentOverlayTile == end)
                {
                    // final path
                    Debug.Log("[FindPath]: PathFinder starts to return the final path.");
                    return GetFinishedList(start, end);
                }

                List<BoardUnit> neighbourTiles = GetNeighbourBoardUnit(currentOverlayTile, new List<BoardUnit>());
                if (neighbourTiles.Count == 0)
                {
                    Debug.Log("No Neighbour");
                }
                foreach (var neighbour in neighbourTiles)
                {
            
                    if (neighbour.isBlocked || closeList.Contains(neighbour) || Mathf.Abs(neighbour.gridLocation.z - currentOverlayTile.gridLocation.z) > 1)
                    {
                        continue;
                    }
            
                    // Cal the manhattan distance
                    Debug.Log("[FindPath]: PathFinder starts to cal all the neighbours.");
                    neighbour.pathf_G = GetManhattenDistance(start, neighbour);
                    neighbour.pathf_H = GetManhattenDistance(end, neighbour);
                    neighbour.previousBoardUnit = currentOverlayTile;

                    if (!openList.Contains(neighbour))
                    {
                        Debug.Log("[FindPath]: PathFinder starts to add all the neighbours.");
                        openList.Add(neighbour);
                    }
                }

            }
            // Empty if it is invalid
            Debug.Log("[FindPath]: PathFinder return the empty path.");
            return new List<BoardUnit>();
            
        }

        public List<BoardUnit> GetFinishedList(BoardUnit start, BoardUnit end)
        {
            List<BoardUnit> finishedList = new List<BoardUnit>();
            BoardUnit current = end;
            while (current != start)
            {
                finishedList.Add(current);
                current = current.previousBoardUnit;
                Debug.Log(current.gridLocation);
            }

            finishedList.Reverse();
            return finishedList;
        }
        
        public int GetManhattenDistance(BoardUnit tile1, BoardUnit tile2)
        {
            return Mathf.Abs(tile1.gridLocation.x - tile2.gridLocation.x) +
                   Mathf.Abs(tile1.gridLocation.y - tile2.gridLocation.y);
        }

        public List<BoardUnit> GetNeighbourBoardUnit(BoardUnit current, List<BoardUnit> searchableUnits)
        {

            Dictionary<Vector2Int, BoardUnit> unitsToSearch = new Dictionary<Vector2Int, BoardUnit>();

            if (searchableUnits.Count > 0)
            {
                foreach (BoardUnit item in searchableUnits)
                {
                    unitsToSearch.Add(item.grid2DLocation, item);
                }
            }
            else
            {
                // Get all the tile in our map
                // -> map is initialized when the scene is loaded
               // Dictionary<Vector2Int, BoardUnit> map = fightBoard_dict;
                unitsToSearch = fightBoard_dict;
            }


            
            List<BoardUnit> neighbours = new List<BoardUnit>();
            // Get the tile's location in the grid -> easier to loop all the neighbours
      
            // [TODO]: change the logic here
            // y+1 -> top neigbour
            Vector2Int locationToCheck = new Vector2Int(
                current.gridLocation.x, 
                current.gridLocation.y + 1);
            if (unitsToSearch.ContainsKey(locationToCheck))
            {
                if(Mathf.Abs(current.gridLocation.z - unitsToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(unitsToSearch[locationToCheck]);
            }
      
      
            // bottom neigbour
            locationToCheck = new Vector2Int(
                current.gridLocation.x, 
                current.gridLocation.y - 1);
            if (unitsToSearch.ContainsKey(locationToCheck))
            {
                if(Mathf.Abs(current.gridLocation.z - unitsToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(unitsToSearch[locationToCheck]);
            }
      
            // right neigbour
            locationToCheck = new Vector2Int(
                current.gridLocation.x + 1, 
                current.gridLocation.y);
            if (unitsToSearch.ContainsKey(locationToCheck))
            {
                if(Mathf.Abs(current.gridLocation.z - unitsToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(unitsToSearch[locationToCheck]);
            }
      
      
            // left neigbour
            locationToCheck = new Vector2Int(
                current.gridLocation.x - 1, 
                current.gridLocation.y);
            if (unitsToSearch.ContainsKey(locationToCheck))
            {
                if(Mathf.Abs(current.gridLocation.z - unitsToSearch[locationToCheck].gridLocation.z) <= 1)
                neighbours.Add(unitsToSearch[locationToCheck]);
            }
            
            return neighbours;

        }
        
        
        //  Show Range.
        public List<BoardUnit> ShowInRangUnit(int range)
        {
            List<BoardUnit> inRangeUnit = GetBoardUnitsInRange(Player.Instance.PlayerActiveBoardUnit, range);
            foreach (var item in inRangeUnit)
            {
                item.showBoardUnit();
            }

            return inRangeUnit;
        }
        
        public void HideInRangUnit(int range)
        {
            List<BoardUnit> inRangeUnit = GetBoardUnitsInRange(Player.Instance.PlayerActiveBoardUnit, range);
            foreach (var item in inRangeUnit)
            {
                item.hideBoardUnit();
            }

            
        }
        
        

        public List<BoardUnit> GetBoardUnitsInRange(BoardUnit startingUnit, int range)
        {
            List<BoardUnit> inRangeUnit = new List<BoardUnit>();
            int stepCount = 0;
            
            inRangeUnit.Add((startingUnit));

            List<BoardUnit> unitForPreviousStep = new List<BoardUnit>();
            unitForPreviousStep.Add(startingUnit);

            while (stepCount < range)
            {
                List<BoardUnit> surroundingTiles = new List<BoardUnit>();
                foreach (var VARIABLE in unitForPreviousStep)
                {
                    surroundingTiles.AddRange(GetNeighbourBoardUnit(VARIABLE, new List<BoardUnit>()));
                }
                
                inRangeUnit.AddRange(surroundingTiles);
                unitForPreviousStep = surroundingTiles.Distinct().ToList();
                stepCount++;
            }

            return inRangeUnit.Distinct().ToList();
        }
}
