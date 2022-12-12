using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;
/// <summary>
/// To manage all the game configuration table
/// </summary>
public class GameConfigManager
{
    public static GameConfigManager Instance = new GameConfigManager();

    private GameConfigData _cardData;

    private GameConfigData _enemyData;

    private GameConfigData _levelData;

    private GameConfigData _cardTypeData;

    private GameConfigData _playerData;
    
    //  [TODO] Not Implemented
    private GameConfigData _chipData;

    //private GameConfigData _cardTypeData;
    private TextAsset _textAsset;
    
    //  Load the txt file into the game
    public void Init()
    {
        _textAsset = Resources.Load<TextAsset>("Data/card");
        _cardData = new GameConfigData(_textAsset.text);
        
        _textAsset = Resources.Load<TextAsset>("Data/enemy");
        _enemyData = new GameConfigData(_textAsset.text);
        
        _textAsset = Resources.Load<TextAsset>("Data/level");
        _levelData = new GameConfigData(_textAsset.text);

        _textAsset = Resources.Load<TextAsset>("Data/cardType");
        _cardTypeData = new GameConfigData(_textAsset.text);

        _textAsset = Resources.Load<TextAsset>("Data/player");
        _playerData = new GameConfigData((_textAsset.text));

    }

    public List<Dictionary<string, string>> GetCardLines()
    {
        return _cardData.GetLinesOfDataDict();
    }
    
    
    public List<Dictionary<string, string>> GetEnemyLines()
    {
        return _enemyData.GetLinesOfDataDict();
    }
    
    
    public List<Dictionary<string, string>> GetLevelLines()
    {
        return _levelData.GetLinesOfDataDict();
    }

    public List<Dictionary<string, string>> GetPlaterLines()
    {
        return _playerData.GetLinesOfDataDict();
    }

    public Dictionary<string, string> GetPlayerInfoById(string id)
    {
        return _playerData.GetOneItemById(id);
    }

    public Dictionary<string, string> GetCardInfoById(string id)
    {
        return _cardData.GetOneItemById(id);
    }
    
    public Dictionary<string, string> GetEnemyInfoById(string id)
    {
        return _enemyData.GetOneItemById(id);
    }
    
    public Dictionary<string, string> GetLevelInfoById(string id)
    {
        return _levelData.GetOneItemById(id);
    }

    public Dictionary<string, string> GetCardTypeInfoById(string id)
    {
        return _cardTypeData.GetOneItemById(id);
    }




}
