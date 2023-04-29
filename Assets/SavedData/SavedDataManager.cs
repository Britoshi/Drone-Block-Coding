using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SavedDataManager : MonoBehaviour
{
    private BlockData blockData;
    public BlockData Data { get { return this.blockData; } }
    private List<IDataPersistence> dataPersistenceObjects;
    public static SavedDataManager instance { get; private set; }

    private FileDataHandler fileDataHandler;
    private FileDataHandler programHandler;
    // TODO -- Make it so that the SaveDataManager reads a file that is a JSON containing all the names of the files
    
    string[] savedFiles;


    public void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Debug.LogError("There are moer than 1 SavedDataManagers in the scene!");
    }

    public void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.dataPath, "ProgramList.json");
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
    }

    public void NewProgram()
    {
        this.blockData = new BlockData();
    }

    public void LoadProgram(string program)
    {
        string fileName = program + ".json";
        programHandler = new FileDataHandler(Application.dataPath, fileName);
        // TODO - Load data about the program from the data handler
        // If no data is found, then we'll create a new program
        if(this.blockData == null)
            NewProgram();
        // TODO - Push loaded data to all other scripts that use it
        foreach(IDataPersistence dataPersistenceObject in this.dataPersistenceObjects)
            dataPersistenceObject.LoadProgram(this.blockData);
        Debug.Log("Loaded program " + this.blockData.programName);
    }

    public void SaveProgram()
    {
        // TODO - Pass data to other scripts that need it so they can update it
        foreach(IDataPersistence dataPersistenceObject in this.dataPersistenceObjects)
            dataPersistenceObject.SaveProgram(ref this.blockData);
        // TODO - Save data to a file using the data handler
    }

    public void OnApplicationQuit()
    {
        SaveProgram();
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();
        return dataPersistenceObjects.ToList();
    }
}
