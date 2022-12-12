using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager
{
    public static PlayerManager Instance = new PlayerManager();

    public void LoadPlayerResources(string levelID)
    {
        //  Load the level data from the GameConfigManager
        Dictionary<string, string> levelData = GameConfigManager.Instance.GetLevelInfoById(levelID);
        
        //  Get the information about player's pos.
        string[] playerPos = levelData["PlayerPos"].Split(',');
        
        //  Fetch the position information for board unit.
        int player_x_bu = int.Parse(playerPos[0]);
        int player_y_bu = int.Parse(playerPos[1]);
        
        BoardUnit boardUnit = MapManager.Instance.fightBoard_dict[new Vector2Int(player_x_bu, player_y_bu)];
        if (boardUnit == null)
        {
            Debug.Log("[LoadPlayerResources]: Player's position is invalide!");
        }
        
        //  Get Player information.
        Dictionary<string, string> playerData = GameConfigManager.Instance.GetPlayerInfoById("10001");
        
        //  Instantiate
        GameObject player_obj = Object.Instantiate(Resources.Load(playerData["Model"])) as GameObject;
        if (player_obj == null)
        {
            Debug.Log(playerData["Model"]);
            Debug.Log("[PlayerManager]: This player's Model path is invalid! Get Null");
        }
        
        //  Load the script
        Player player_script = player_obj.AddComponent<Player>();
        player_script.InitPlayer(playerData);
        PositionPlayerOnBoardUnit(boardUnit, player_script);
        
        

    }
    
    
    public void PositionPlayerOnBoardUnit(BoardUnit bu, Player playerScript)
    {
        playerScript.transform.position = bu.transform.position;
        //  Align the sorting order
        playerScript.GetComponent<SpriteRenderer>().sortingOrder = bu.GetComponent<SpriteRenderer>().sortingOrder + 1;
        playerScript.PlayerActiveBoardUnit = bu;
    }

}
