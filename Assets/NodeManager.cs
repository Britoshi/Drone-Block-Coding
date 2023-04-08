using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeManager : MonoBehaviour
{
    Button node_button, compile_button;
    GameObject node_container;

    [SerializeField]
    GameObject node_prefab;    
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform child in transform)
        {
            if(child.gameObject.name == "NodeButton")
            {
                node_button = child.GetComponent<Button>();
                node_button.onClick.AddListener(delegate { CreateNode(); });
            }
            if(child.gameObject.name == "CompileButton")
            {
                compile_button = child.GetComponent<Button>();
                compile_button.onClick.AddListener(delegate { Compile(); });
            }
            if(child.gameObject.name == "Nodes")
            {
                node_container = child.gameObject;
            }
        }
    }

    public void CreateNode()
    {
        GameObject node = Instantiate(node_prefab);
        node.transform.SetParent(node_container.transform);
        node.transform.localScale = new Vector3(1.2917f, 1.2917f, 1.2917f);
        node.transform.GetComponent<RectTransform>().position = new Vector3(-4.40f, 15, 0);
    }

    public void Compile()
    {
        foreach(Transform child in node_container.transform)
        {
            Debug.Log(child.gameObject.name);
        }
    }
}

