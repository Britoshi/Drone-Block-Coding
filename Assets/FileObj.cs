using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FileObj : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI text;
    string Name {get {return text.text;} set {text.text = value;}}

    public MenuManager menuManager;


    public void Populate(MenuManager manager, string fileName)
    {
        menuManager = manager;
        Name = fileName;
        foreach(Transform child in transform)
        {
            try
            {
                Button button = child.GetComponent<Button>();
                button.onClick.AddListener(delegate { menuManager.LoadProgram(Name); });
            }
            catch(System.Exception e)
            {
                Debug.Log("Error adding button to file: " + e.Message);
            }
        }
    }
}
