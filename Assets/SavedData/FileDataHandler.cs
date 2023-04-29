using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class FileDataHandler 
{
    private string dataDirPath = "";
    private string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    public BlockData Load()
    {
        string dataFilePath = Path.Combine(this.dataDirPath, this.dataFileName);
        try
        {
            // Read the file from the file system
            using(FileStream stream = new FileStream(dataFilePath, FileMode.Open, FileAccess.Read))
            {
                using(StreamReader reader = new StreamReader(stream))
                {
                    string dataToLoad = reader.ReadToEnd();
                    // Deserialize from JSON
                    BlockData data = JsonUtility.FromJson<BlockData>(dataToLoad);
                    return data;
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error loading data from file " + dataFilePath + ": " + e.Message);
            return null;
        }
    }

    public void Save(BlockData data)
    {
        string dataFilePath = Path.Combine(this.dataDirPath, this.dataFileName);
        try
        {
            // Create the directory path
            Directory.CreateDirectory(Path.GetDirectoryName(dataFilePath));
            // Serialize to JSON
            string dataToStore = JsonUtility.ToJson(data, true);

            // Write the file to the file system
            using(FileStream stream = new FileStream(dataFilePath, FileMode.Create, FileAccess.Write))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToStore);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Error saving data to file " + dataFilePath + ": " + e.Message);
        }
    }
}
