using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <Description>
/// The configuration table for this game
/// each object of this class will have its .txt file.
/// <Description>

public class GameConfigData
{
    //  Stores all the data in the game config
    private List<Dictionary<string, string>> _dataDic;

    
    //  Constructor
    public GameConfigData(string str)
    {
        _dataDic = new List<Dictionary<string, string>>();
        
        //  Seperate by line
        string[] lines = str.Split('\n');
        
        //  first line of data is the type of this data
        string[] title = lines[0].Trim().Split('\t');
        
        //  Iterate from 3rd line
        for (int i = 2; i < lines.Length; i++)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            string[] tempArr = lines[i].Trim().Split('\t');

            for (int j = 0; j < tempArr.Length; j++)
            {
                dic.Add(title[j], tempArr[j]);
            }
            
            _dataDic.Add(dic);
        }

    }

    public List<Dictionary<string, string>> GetLinesOfDataDict()
    {
        return _dataDic;
    }


    public Dictionary<string, string> GetOneItemById(string id)
    {
        for (int i = 0; i < _dataDic.Count; i++)
        {
            Dictionary<string, string> dict = _dataDic[i];
            //  Capitalize the i
            if (dict["Id"] == id)
            {
                return dict;
            }
        }

        return null;
    }

}
