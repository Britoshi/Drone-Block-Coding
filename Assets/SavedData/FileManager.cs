using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FileManager : MonoBehaviour
{
    private string _dataDirPath = "";
    private string _dataFileName = "";

    [Header("Data Location")]
    [SerializeField] private string _dataDirName = "DroneBlockData";
    [SerializeField] private LoadableFiles _loadableFiles;
    [SerializeField] private NodeManager nodeManager;
    List<string> _dataFileNames = new List<string>();


    public void Start()
    {
        _dataDirPath = Path.Combine(Application.persistentDataPath, _dataDirName);
        if(Directory.Exists(_dataDirPath) == false)
        {
            Debug.Log("Creating the data directory at " + _dataDirPath);
            Directory.CreateDirectory(_dataDirPath);
        }
        else
        {
            UpdateSavedFiles();
        }
    }

    public void Save(string fileName)
    {
        string dataFilePath = Path.Combine(_dataDirPath, fileName + ".txt");
        try
        {
            if(File.Exists(dataFilePath))
            {
                Debug.Log("File already exists at " + dataFilePath + ", deleting it.");
                File.Delete(dataFilePath);
            }
            // Create the file
            using(FileStream stream = new FileStream(dataFilePath, FileMode.Create, FileAccess.Write))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    nodeManager.updateCommands();
                    foreach(string command in nodeManager.Commands)
                    {
                        writer.WriteLine(command);
                    }
                }
            }
            Debug.Log("Saved data to file " + dataFilePath);
        }
        catch(Exception e)
        {
            Debug.LogError("Error saving data to file " + dataFilePath + ": " + e.Message);
        }
    }

    public void UpdateSavedFiles()
    {
        // Delete the files from the LoadableFiles object
        _loadableFiles.DeleteFiles();
        Debug.Log("Data directory already exists at " + _dataDirPath);
        DirectoryInfo d = new DirectoryInfo(_dataDirPath);
        FileInfo[] Files = d.GetFiles("*.txt");
        foreach(FileInfo file in Files)
        {
            Debug.Log("Found file: " + file.Name);
            _loadableFiles.AddFile(file.Name.Substring(0, file.Name.Length - 4));   
        }
    }

    // Loading is handled by the NodeManager & MenuManager
}
