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
    

    int currNodeIndex = 0;
    SortedDictionary<int, Vector2> positions = new SortedDictionary<int, Vector2>();
    // Start is called before the first frame update
    void Start()
    {  
            compileButton.onClick.AddListener(Compile);  
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
        newNode.GetComponent<Node>().index = currNodeIndex;
        positions.Add(currNodeIndex, newNode.GetComponent<RectTransform>().anchoredPosition);
        currNodeIndex++;
    }

    public void Compile()
    {
        foreach(Transform child in nodeContainer.transform)
        {
            Debug.Log(child.gameObject.name);
        }
    }

    public void ProgramNodeMovement(GameObject tempObj, Vector2 position)
    {
        if(position.y > tempObj.GetComponent<RectTransform>().anchoredPosition.y)
        {
            tempObj.transform.SetSiblingIndex(tempObj.transform.GetSiblingIndex() + 1);
            return;
        }
        else
        {
            tempObj.transform.SetSiblingIndex(tempObj.transform.GetSiblingIndex() - 1);
            return;
        }
    }

    public void UpdateNodePositions(int originalIndex, int newIndex)
    {
        if(newIndex > positions.Count - 1)
        {
            newIndex--;
        }
        if (newIndex < 0)
        {
            newIndex = 0;
        }

        Debug.Log($"Original Index: {originalIndex} New Index: {newIndex}");
        Vector2 temp = positions[originalIndex];
        positions[originalIndex] = positions[newIndex];
        positions[newIndex] = temp;
        nodeContainer.GetChild(originalIndex).SetSiblingIndex(newIndex);
        nodeContainer.GetChild(newIndex + 1).SetSiblingIndex(originalIndex);
    }
    /*
    {
        // We'll actually just take the index of the temp object and set the sibling index of the node to that
        Vector2 currentNodePosition = Vector2.zero;
        for(int i = 0; i < nodeContainer.childCount; i++)
        {
            currentNodePosition = positions[i];
            for(int j = 0; j < nodeContainer.childCount; j++)
            {
                if(i != j)
                {
                    if(currentNodePosition.y > positions[j].y)
                    {
                        nodeContainer.GetChild(i).SetSiblingIndex(j);
                        Vector2 temp = positions[i];
                        positions[i] = positions[j];
                        positions[j] = temp;
                        break;
                    }
                    if(currentNodePosition.y < positions[j].y)
                    {
                        nodeContainer.GetChild(i).SetSiblingIndex(j);
                        Vector2 temp = positions[i];
                        positions[i] = positions[j];
                        positions[j] = temp;
                        break;
                    }
                }
            }           
        }
    }
    */


}

