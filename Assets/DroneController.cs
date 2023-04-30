using System.Collections;
using System.Collections.Generic; 
using UnityEngine;

using TelloCommander.CommandDictionaries;
using TelloCommander.Commander;
using TelloCommander.Connections;
using TelloCommander.Interfaces;
using System.IO;
using System;
using UnityEditor.VersionControl;
using UnityEditor;

public class DroneController : BritoBehavior
{
    CommandDictionary dictionary;
    DroneCommander commander;

    string CONTENT_DIR => Application.persistentDataPath + "/Content";
     

    [SerializeField]
    List<Asset> files;

    private void Awake()
    {
        if (!Directory.Exists(CONTENT_DIR))
        {
            Directory.CreateDirectory(CONTENT_DIR);
            foreach (var file in files)
            {
                // Get the file path for this asset
                string filePath = Path.Combine(CONTENT_DIR, file.fullName);




                //AssetDatabase.

                // Write the contents of the asset to the file
                //File.WriteAllBytes(filePath, file.Load().);
            }
        } 
    }

    // Start is called before the first frame update
    void Start()
    { 
        dictionary = CommandDictionary.ReadStandardDictionary("1.3.0.0");
        print("Imported Dictionary");
        commander = new DroneCommander(new TelloConnection(), dictionary);
        print("Initiated Drone Commander");
        commander.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Execute());
        }
    }

    IEnumerator Execute()
    {
        
        commander.RunCommand("takeoff");
        yield return new WaitForSecondsRealtime(.1f);
        commander.RunCommand("forward 20");
        yield return new WaitForSecondsRealtime(.1f);

        commander.RunCommand("land");
        yield return new WaitForSecondsRealtime(.1f);
    }

}
