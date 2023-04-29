using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;

public class NodeManager : MonoBehaviour, IDataPersistence
{
    [Header("Properties")]
    [SerializeField] Transform nodeContainer;
    [SerializeField] GameObject nodePrefab; 
    int _numNodes = 0;   
    private bool _fakePresent = false;
    List<string> _commands = new List<string>();
    
    // INTERFACE METHODS
    public void LoadProgram(BlockData data)
    {
        _commands = data.blockNames;
    }

    public void SaveProgram(ref BlockData data)
    {
        data.blockNames = _commands;
    }


    // CLASS METHODS

    void Start()
    {
        //LoadProgram(data);
    }
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


    public void Compile()
    {
        List<string> commands = new List<string>();
        foreach(Transform child in nodeContainer.transform)
        {
            commands.Add(child.gameObject.name);
        }
        DroneConnectionMaster.SendCommands(commands);
    }

    public void NodeRemoved(GameObject node)
    {
        _commands.Remove(node.name);   
    }

    public void NodeUpdated(string oldName, string newName)
    {
        _commands.Remove(oldName);
        _commands.Add(newName);
    }

    public void AddNode(string nodeName)
    {
        _commands.Add(nodeName);
    }


}

