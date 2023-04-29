using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.UI.Extensions;

public class NodeManager : BritoBehavior
{
    [Header("Properties")]
    [SerializeField] Transform nodeContainer;
    [SerializeField] GameObject nodePrefab; 
    int _numNodes = 0;   
    private bool _fakePresent = false;
    

    // Start is called before the first frame update
    void Start()
    {  
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
        print("Untemplating");
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
}

