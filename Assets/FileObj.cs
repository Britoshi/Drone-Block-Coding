using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FileObj : MonoBehaviour
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI text;
    string Name {get {return text.text;} set {text.text = value;}}

    public MenuManager menuManager;


    public void Populate(MenuManager manager, string fileName)
    {
        menuManager = manager;
        Name = fileName;
        button.onClick.AddListener(delegate {menuManager.LoadProgram(Name);});
    }
}
