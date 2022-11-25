using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinder : MonoBehaviour
{
   public List<OverlayTile> FindPath(OverlayTile start, OverlayTile End)
   {
      // Can we use a PriorityQueue instead?
      // Init
      List<OverlayTile> openList = new List<OverlayTile>();
      List<OverlayTile> closeList = new List<OverlayTile>();
      openList.Add(start);
      
      Debug.Log("[FindPath]: PathFinder starts to work.");
      // Find the shortest path
      while (openList.Count > 0)
      {
         Debug.Log("[FindPath]: PathFinder in the while loop.");
         // Get the node with the smallest F cost.
         OverlayTile currentOverlayTile = openList.OrderBy(x => x.pathf_F).First();
         openList.Remove(currentOverlayTile);
         closeList.Add(currentOverlayTile);
         if (currentOverlayTile == End)
         {
            // final path
            Debug.Log("[FindPath]: PathFinder starts to return the final path.");
            return GetFinishedList(start, End);
         }

         List<OverlayTile> neighbourTiles = GetNeighbourTiles(currentOverlayTile);
         foreach (var neighbour in neighbourTiles)
         {
            
            if (neighbour.isBlocked || closeList.Contains(neighbour) || Mathf.Abs(neighbour.gridLocation.z - currentOverlayTile.gridLocation.z) > 1)
            {
               continue;
            }
            
            // Cal the manhattan distance
            Debug.Log("[FindPath]: PathFinder starts to cal all the neighbours.");
            neighbour.pathf_G = GetManhattenDistance(start, neighbour);
            neighbour.pathf_H = GetManhattenDistance(End, neighbour);
            neighbour.previousTile = currentOverlayTile;

            if (!openList.Contains(neighbour))
            {
               Debug.Log("[FindPath]: PathFinder starts to add all the neighbours.");
               openList.Add(neighbour);
            }
         }

      }
      // Empty if it is invalid
      Debug.Log("[FindPath]: PathFinder return the empty path.");
      return new List<OverlayTile>();
   }

   public List<OverlayTile> GetFinishedList(OverlayTile Start, OverlayTile End)
   {
      List<OverlayTile> finishedList = new List<OverlayTile>();
      OverlayTile current = End;
      while (current != Start)
      {
         finishedList.Add(current);
         current = current.previousTile;
         Debug.Log(current.gridLocation);
      }

      finishedList.Reverse();
      return finishedList;
   }

   public int GetManhattenDistance(OverlayTile tile1, OverlayTile tile2)
   {
      return Mathf.Abs(tile1.gridLocation.x - tile2.gridLocation.x) +
             Mathf.Abs(tile1.gridLocation.y - tile2.gridLocation.y);
   }

   public List<OverlayTile> GetNeighbourTiles(OverlayTile current)
   {
      // Get all the tile in our map
      // -> map is initialized when the scene is loaded
      Dictionary<Vector2Int, OverlayTile> map = MapManager.Instance.map;
      List<OverlayTile> neighbours = new List<OverlayTile>();
      // Get the tile's location in the grid -> easier to loop all the neighbours
      
      // [TODO]: change the logic here
      // y+1 -> top neigbour
      Vector2Int locationToCheck = new Vector2Int(
         current.gridLocation.x, 
         current.gridLocation.y + 1);
      if (map.ContainsKey(locationToCheck))
      {
         neighbours.Add(map[locationToCheck]);
      }
      
      
      // bottom neigbour
      locationToCheck = new Vector2Int(
         current.gridLocation.x, 
         current.gridLocation.y - 1);
      if (map.ContainsKey(locationToCheck))
      {
         neighbours.Add(map[locationToCheck]);
      }
      
      // right neigbour
      locationToCheck = new Vector2Int(
         current.gridLocation.x + 1, 
         current.gridLocation.y);
      if (map.ContainsKey(locationToCheck))
      {
         neighbours.Add(map[locationToCheck]);
      }
      
      
      // left neigbour
      locationToCheck = new Vector2Int(
         current.gridLocation.x - 1, 
         current.gridLocation.y);
      if (map.ContainsKey(locationToCheck))
      {
         neighbours.Add(map[locationToCheck]);
      }
      
      
      return neighbours;

   }
   
   
   
   
   
   
}
