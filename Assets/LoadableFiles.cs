using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadableFiles : MonoBehaviour
{
    [SerializeField] GameObject filePrefab;
    [SerializeField] GridLayoutGroup gridLayoutGroup;
    [SerializeField] MenuManager menuManager;
    public Vector2 startingSize;

    
    void Start()
    {
        startingSize = gridLayoutGroup.GetComponent<RectTransform>().sizeDelta;
    }

    public void AddFile(string fileName)
    {
        GameObject newFile = Instantiate(filePrefab, gridLayoutGroup.transform);
        newFile.GetComponent<FileObj>().Populate(menuManager, fileName);
        gridLayoutGroup.GetComponent<RectTransform>().sizeDelta = new Vector2(gridLayoutGroup.GetComponent<RectTransform>().sizeDelta.x, gridLayoutGroup.GetComponent<RectTransform>().sizeDelta.y + newFile.GetComponent<RectTransform>().sizeDelta.y);

    }

    public void LoadFile(string fileName)
    {
        Debug.Log("Loading file: " + fileName);
        menuManager.LoadProgram(fileName);
    }

    public void DeleteFiles()
    {
        foreach(Transform child in gridLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }
        gridLayoutGroup.GetComponent<RectTransform>().sizeDelta = startingSize;
    }
}
