using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Data;
using Excel;


public static class MyEditor
{
    [MenuItem("MyTools/ExcelToTxt")]
    
    
    public static void ExportExcelToTxt()
    {
        //  The path to the Excel folder
        string assestPath = Application.dataPath + "/_Excel";
        
        //  Load the Excle file from the folder
        string[] files = Directory.GetFiles(assestPath, "*.xlsx");
        if (files.Length == 0)
        {
            Debug.Log("[ExportExcelToTxt]:  No Excel file found in the given folder!");
        }

        for (int i = 0; i < files.Length; i++)
        {
            //  Formatting correctly
            files[i] = files[i].Replace('\\', '/');


            using (FileStream fs = File.Open(files[i], FileMode.Open, FileAccess.Read))
            {
                //  Convert the FileStream to Excel
                var excelDataReader = ExcelReaderFactory.CreateOpenXmlReader(fs);
                
                //  Fetch the Excel Data
                DataSet dataSet = excelDataReader.AsDataSet();
                
                //  Access the 1st table in the Excel file.
                DataTable table = dataSet.Tables[0];
                
                //  Store it as the corresponding txt file.
                readTableToTxt(files[i], table);
            }
            
            
        }
        //  This is to refresh the editor
        AssetDatabase.Refresh();
    }


    private static void readTableToTxt(string FilePath, DataTable table)
    {
        //  Get only the name of the directory used for the name of the txt file
        string fileName = Path.GetFileNameWithoutExtension(FilePath);
        
        //  The Path where the txt file should be stored
        string targetFilePath = Application.dataPath + "/Resources/Data/" + fileName + ".txt";
        
        //  Check for repeating file and delete the old file if exists.
        if (File.Exists(targetFilePath))
        {
            Debug.Log("[readTableToTxt]: There is a duplicate txt file to be destroyed！");
            File.Delete(targetFilePath);
        }
        
        //  Create a new file using FileStream
        using (FileStream fs = new FileStream(targetFilePath, FileMode.Create))
        {
            //  Convert the FileStream to StreamWriter for convenience
            using (StreamWriter sw = new StreamWriter(fs))
            {
                //  Iterate through the Data Table
                for (int row = 0; row < table.Rows.Count; row++)
                {

                    DataRow dataRow = table.Rows[row];
                    string temp = "";

                    for (int col = 0; col < table.Columns.Count; col++)
                    {
                        string tableVal = dataRow[col].ToString();
                        temp = temp + tableVal + "\t";  //  Sep each with a tab
                    }
                    
                    //  Write
                    sw.Write(temp);
                    
                    //  Change the line or finish
                    if (row != table.Rows.Count - 1)
                    {
                        sw.WriteLine();
                    }

                }
                
            }
            
            
        }

    }
}
