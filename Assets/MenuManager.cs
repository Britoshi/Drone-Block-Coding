using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] GameObject loadMenu;
    [SerializeField] GameObject mainCanvas;
    [SerializeField] GameObject programCanvas;
    [SerializeField] NodeManager nodeManager;

    public FileObj[] savedFiles;

    void Awake()
    {
        // We'll load how many saved files there are and then populate the files
        if(loadMenu != null)
        {

        }
        // If the loadMenu is null, then we won't load any files whatsoever
    }
    public void LoadProgram(string programName)
    {
        if(programName == "New")
            SwitchScreen(programCanvas);
        else
        {
            // But now we need to load the program specified by passing along the filename of the program
            SwitchScreen(programCanvas);
            SavedDataManager.instance.LoadProgram(programName);
            nodeManager.LoadProgram(SavedDataManager.instance.Data);
        }
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
            }
            else if(screen == programCanvas)
            {
                mainCanvas.SetActive(false);
                programCanvas.SetActive(true);
            }
        }
    }
}
