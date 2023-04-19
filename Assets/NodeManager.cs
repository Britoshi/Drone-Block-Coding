using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{

    [Header("Properties")]
    [SerializeField] Button compileButton;
    [SerializeField] Transform nodeContainer;

    [SerializeField] GameObject nodePrefab;    
    // Start is called before the first frame update
    void Start()
    {  
            compileButton.onClick.AddListener(Compile);  
            print(transform.Find("BG"));
    }

    public void CreateNode()
    {
        Vector2 position = Vector2.zero;
        Transform lastNode;
        if(nodeContainer.childCount != 0)
        {
            lastNode = nodeContainer.GetChild(nodeContainer.childCount - 1); 
            position = lastNode.position;
        }        
        GameObject node = Instantiate(nodePrefab, nodeContainer);   
    }

    public void Compile()
    {
        foreach(Transform child in nodeContainer.transform)
        {
            Debug.Log(child.gameObject.name);
        }
    }
}

