using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NodeManager : BritoBehavior
{
    [Header("Properties")]
    [SerializeField] Transform nodeContainer;
    [SerializeField] GameObject nodePrefab;    

    

    // Start is called before the first frame update
    void Start()
    {  
            print(transform.Find("BG"));
    }

    public void CreateNode(GameObject node)
    {
        Vector2 currentProgramField = nodeContainer.GetComponent<RectTransform>().sizeDelta;
        Vector2 currentGameObjectField = node.GetComponent<RectTransform>().sizeDelta;
        nodeContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(currentProgramField.x, currentProgramField.y + currentGameObjectField.y + 10);
        Vector2 position = Vector2.zero;
        Transform lastNode;

        if(nodeContainer.childCount != 0)
        {
            lastNode = nodeContainer.GetChild(nodeContainer.childCount - 1); 
            position = lastNode.position;
        }        

        GameObject newNode = Instantiate(node, nodeContainer);
        newNode.GetComponent<Node>().isDraggable = true;
    }

    public void Compile()
    {
        List<string> commands = new List<string>();
        foreach(Transform child in nodeContainer.transform)
        {
            commands.Add(child.gameObject.name);
        }
        //DroneConnectionMaster.SendCommands(commands);
    }

}

