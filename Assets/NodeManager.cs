using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;
using System;
using UnityEngine.UIElements;

public class NodeManager : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] Transform nodeContainer;
    [SerializeField] GameObject nodePrefab;
    [SerializeField] Color[] frameColors; 
    [SerializeField] GameObject runningScreen;
    int _numNodes = 0;   
    private bool _fakePresent = false;
    List<string> _commands = new List<string>();
    public List<string> Commands {get {return _commands;} set {_commands = value;}}

    Vector2 _contentSize;
    
    void Start()
    {
        _contentSize = nodeContainer.GetComponent<RectTransform>().sizeDelta;
        DroneController.AddOnStartFunction(OnStartCommand);
        DroneController.AddOnEndFunction(OnCommandEnd);
    }

    // CLASS METHODS
    void Update()
    {
        if(nodeContainer.childCount > _numNodes)
        {
            foreach(Transform child in nodeContainer.transform)
            {
                if(child.gameObject.name == "Fake")
                {
                    _fakePresent = true;
                }
            }
            if(_fakePresent)
            {
                _fakePresent = false;
                return;
            }
            _numNodes = nodeContainer.childCount;
            AddNode("New Node");
            Untemplate();
        }
        else if(nodeContainer.childCount < _numNodes)
        {
            _numNodes = nodeContainer.childCount;
        }
    }

    public void Untemplate()
    {
        foreach(Transform child in nodeContainer.transform)
        {
            Node node = child.GetComponent<Node>();
            node.isTemplate = false; 
            node.deleteButton.transform.gameObject.SetActive(true);

            // Only set the increase button to be active since we will be at minimum by default
            node.increaseButton.transform.gameObject.SetActive(true);
        }
    }

    public void OnStartCommand()
    {
        runningScreen.SetActive(true);
    }

    public void OnCommandEnd()
    {
        runningScreen.SetActive(false);
    }


    // This function is where we'll have any of the error checking to ensure we don't go over the limits in place
    // FOR THE DEMO: Only working limit is with height.
    public void Compile()
    {
        int maxHeight = PlayerPrefs.GetInt("maxHeight");
        int maxRadius = PlayerPrefs.GetInt("maxRadius");
        int currentHeight = 10;
        runningScreen.SetActive(true);
        List<string> commands = new List<string>();
        foreach(Transform child in nodeContainer.transform)
        {
            string command = child.gameObject.name;
            if(command.Contains("up"))
            {
                currentHeight += Convert.ToInt32(command.Substring(command.IndexOf(" ")).Trim());
                if (currentHeight > maxHeight)
                {
                    continue;
                }
            }
            commands.Add(child.gameObject.name);
        }
        DroneController.RunCommands(commands);
    }

    public void NodeRemoved(GameObject node)
    {
        _commands.Remove(node.name); 
        nodeContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(nodeContainer.GetComponent<RectTransform>().sizeDelta.x, nodeContainer.GetComponent<RectTransform>().sizeDelta.y - ( node.GetComponent<RectTransform>().sizeDelta.y * 1f) + nodeContainer.gameObject.GetComponent<GridLayoutGroup>().spacing.y);  
    }

    public void NodeUpdated(string oldName, string newName)
    {
        _commands.Remove(oldName);
        _commands.Add(newName);
    }

    public void AddNode(string nodeName)
    {
        _commands.Add(nodeName);
        try
        {
            GameObject node = nodeContainer.GetComponentInChildren<Node>().gameObject;
            nodeContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(nodeContainer.GetComponent<RectTransform>().sizeDelta.x, nodeContainer.GetComponent<RectTransform>().sizeDelta.y + ( node.GetComponent<RectTransform>().sizeDelta.y * 1f) + nodeContainer.gameObject.GetComponent<GridLayoutGroup>().spacing.y);
        }
        catch(Exception e)
        {
            Debug.Log("No nodes present | Exception: " + e);
        }
    }

    public void CreateNode(string command)
    {
        GameObject newNode = Instantiate(nodePrefab, nodeContainer);
        newNode.name = command;
        Node newNodeScript = newNode.GetComponent<Node>();
        newNodeScript.value = Convert.ToInt32(command.Substring(command.IndexOf(" ")));
        newNodeScript.nodeManager = this;
        string commandName = command.Substring(0, command.IndexOf(" "));
        // Use the frameColors array for the colors
        // Colors are as follows:
        /*
            0 -- F, B, L, R
            1 -- U, D
            2 -- CW, CCW
        */
        
        switch(commandName)
        {
            case "forward":
                newNodeScript.iconSprite = Resources.Load<Sprite>("Sprites/Icons/forward");
                newNodeScript.frameColor = frameColors[0];
                newNodeScript.nodeCommand = "forward";
                break;
            case "back":
                newNodeScript.iconSprite = Resources.Load<Sprite>("Sprites/Icons/back");
                newNodeScript.frameColor = frameColors[0];
                newNodeScript.nodeCommand = "back";
                break;
            case "left":
                newNodeScript.iconSprite = Resources.Load<Sprite>("Sprites/Icons/left");
                newNodeScript.frameColor = frameColors[0];
                newNodeScript.nodeCommand = "left";
                break;
            case "right":
                newNodeScript.iconSprite = Resources.Load<Sprite>("Sprites/Icons/right");
                newNodeScript.frameColor = frameColors[0];
                newNodeScript.nodeCommand = "right";
                break;
            case "up":
                newNodeScript.iconSprite = Resources.Load<Sprite>("Sprites/Icons/up");
                newNodeScript.frameColor = frameColors[1];
                newNodeScript.nodeCommand = "up";
                break;
            case "down":
                newNodeScript.iconSprite= Resources.Load<Sprite>("Sprites/Icons/down");
                newNodeScript.frameColor = frameColors[1];
                newNodeScript.nodeCommand = "down";
                break;
            case "cw":
                newNodeScript.iconSprite = Resources.Load<Sprite>("Sprites/Icons/clockwise");
                newNodeScript.frameColor = frameColors[2];
                newNodeScript.nodeCommand = "cw";
                break;
            case "ccw":
                newNodeScript.iconSprite = Resources.Load<Sprite>("Sprites/Icons/counterclockwise");
                newNodeScript.frameColor = frameColors[2];
                newNodeScript.nodeCommand = "ccw";
                break;
            default:
                break;
        }
        // Literally don't know why, but this needed to be in a coroutine to work
        // All this does is update the buttons on the nodes to be appropriate
        StartCoroutine(UpdateNodeButtons(newNodeScript));


        // Still need some more info, but this should be okay to test

        AddNode(command);
    }

    IEnumerator UpdateNodeButtons(Node nodeScript)
    {
        yield return new WaitForSeconds(0.01f);
        nodeScript.ValueUpdate("=");
    }

    public void DeleteAllNodes()
    {
        foreach(Transform child in nodeContainer.transform)
        {
            child.GetComponent<Node>().DeleteNode();
        }
        nodeContainer.GetComponent<RectTransform>().sizeDelta = _contentSize;
        _commands.Clear();
    }

    public void updateCommands()
    {
        _commands.Clear();
        foreach(Transform child in nodeContainer.transform)
        {
            _commands.Add(child.gameObject.name);
        }
    }
}

