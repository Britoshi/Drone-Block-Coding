using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadableFiles : MonoBehaviour
{
    [SerializeField] GameObject filePrefab;
    [SerializeField] Transform fileContainer;
    [SerializeField] MenuManager menuManager;
    public Vector2 startingSize;
    GridLayoutGroup gridLayoutGroup;
    
    void Start()
    {
        gridLayoutGroup = fileContainer.GetComponent<GridLayoutGroup>();
        startingSize = gridLayoutGroup.GetComponent<RectTransform>().sizeDelta;
    }

    public void AddFile(string fileName)
    {
        Debug.Log("Adding file: " + fileName);
        GameObject newFile = Instantiate(filePrefab, fileContainer);
        newFile.GetComponent<FileObj>().Populate(menuManager, fileName);
    }

    public void LoadFile(string fileName)
    {
        Debug.Log("Loading file: " + fileName);
        menuManager.LoadProgram(fileName);
    }

    public void DeleteFiles()
    {
        foreach(Transform child in fileContainer)
        {
            Destroy(child.gameObject);
        }
        fileContainer.GetComponent<RectTransform>().sizeDelta = startingSize;
    }

    public void ResizeContainer()
    {
        Debug.Log("Resizing container");
        foreach(Transform child in fileContainer)
        {
            fileContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(fileContainer.GetComponent<RectTransform>().sizeDelta.x, fileContainer.GetComponent<RectTransform>().sizeDelta.y + (fileContainer.GetComponent<GridLayoutGroup>().cellSize.y / 2));
        }
    }

}
