using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FileObj : MonoBehaviour
{
    Button button;
    TextMeshProUGUI text;
    string Name {get {return text.text;} set {text.text = value;}}

    public MenuManager menuManager;

    void Awake()
    {
        button = GetComponentInChildren<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void Populate(MenuManager manager, string fileName)
    {
        menuManager = manager;
        Name = fileName;
        button.onClick.AddListener(delegate {menuManager.LoadProgram(Name);});
    }
}
