using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System;
using UnityEngine.UI;
using TMPro;
public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject loadMenu;
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject programCanvas;
    [SerializeField] GameObject saveCanvas;
    [SerializeField] GameObject filesCanvas;
    [SerializeField] NodeManager nodeManager;
    [SerializeField] Button saveButton;
    [SerializeField] Button existingSaveButton;
    [SerializeField] TextMeshProUGUI saveName;

    [Header("Properties")]
    [SerializeField] bool debug = false;
    public string currentFile = "";
    public FileObj[] savedFiles;

    void Start()
    {
        saveButton.onClick.AddListener(delegate { SaveBehavior(false); });
        existingSaveButton.onClick.AddListener(delegate { SaveCheck(); });
        if (!debug)
            SwitchScreen(mainCanvas);
    }
    public void LoadProgram(string programName)
    {
        if(programName == "New")
            SwitchScreen(programCanvas);
        else
        {
            // But now we need to load the program specified by passing along the filename of the program
            Debug.Log("Loading program: " + programName);
            SwitchScreen(programCanvas);
            List<string> commands = ReadProgram(programName);
            foreach(string command in commands)
            {
                Debug.Log("Creating node: " + command);
                nodeManager.CreateNode(command);
            }
        }
        currentFile = programName;
    }

    public void ShowGameObject(GameObject menu)
    {
        if(menu != null)
            menu.SetActive(true);
    }

    public void BackGameObject(GameObject menu)
    {
        if(menu != null)
            menu.SetActive(false);
    }

    public void SwitchScreen(GameObject screen)
    {
        if(screen != null)
        {
            if(screen == mainCanvas)
            {
                programCanvas.SetActive(false);
                mainCanvas.SetActive(true);
                saveCanvas.SetActive(false);
                filesCanvas.SetActive(false);
            }
            else if(screen == programCanvas)
            {
                mainCanvas.SetActive(false);
                programCanvas.SetActive(true);
                saveCanvas.SetActive(false);
                filesCanvas.SetActive(false);
            }
            else if(screen == saveCanvas)
            {
                mainCanvas.SetActive(false);
                saveCanvas.SetActive(true);
                programCanvas.SetActive(false);
                filesCanvas.SetActive(false);
            }
            else if (screen == filesCanvas)
            {
                mainCanvas.SetActive(false);
                saveCanvas.SetActive(false);
                programCanvas.SetActive(false);
                filesCanvas.SetActive(true);
            }
        }
    }


    public List<string> ReadProgram(string programName)
    {
        List<string> commands = new List<string>();
        string path = Path.Combine(Application.persistentDataPath, "DroneBlockData", programName + ".txt");
        string[] lines = System.IO.File.ReadAllLines(path);
        foreach(string line in lines)
        {
            commands.Add(line);
        }
        return commands;
    }

    public void SaveBehavior(bool existingFile)
    {
        FileManager fileManager = this.GetComponent<FileManager>();
        if(!existingFile)
        {
            Debug.Log(saveName.text);
            nodeManager.DeleteAllNodes();
            fileManager.Save(saveName.text);
            fileManager.UpdateSavedFiles();
            SwitchScreen(mainCanvas);
        }
        else
        {
            fileManager.Save(currentFile);
            SwitchScreen(mainCanvas);
        }
   }

   public void SaveCheck()
   {
        // If we have a new file, then we want to rename it, otherwise just save the file
        if(currentFile == "New")
        {
            SwitchScreen(saveCanvas);
        }
        else
        {
            nodeManager.DeleteAllNodes();
            SaveBehavior(true);
        }
   }

   public void CreateNewFile()
   {
        currentFile = "New";
        Debug.Log("Creating new file");
        SwitchScreen(programCanvas);
   }
}
