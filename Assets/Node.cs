using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Node : BritoBehavior
{
    // Variables to phase out 
    Button increase;
    Button decrease;

    // Variables to keep
    public int value = 20;  
    Canvas canvas;

    [Header("Outside Scripts")]
    [SerializeField] public NodeManager nodeManager;

    [Header("Node Properties")]
    // Commands possible: F, B, L, R, U, D, CC, C
    [SerializeField] public string nodeCommand;
    [SerializeField] TextMeshProUGUI sizeText;
    [SerializeField] public Button increaseButton;
    [SerializeField] public Button decreaseButton;
    [SerializeField] public Button deleteButton;

    [Header("Image Properties")]
    [SerializeField] Image frameImage;
    [SerializeField] public Image iconImage;
    [SerializeField] public Sprite iconSprite;
    // Possilbe frame colors:
    /*
     #b27b14
     #3e2e4f
     #843021
     #011434
    */
    [SerializeField] public Color frameColor;
    
    [Header("Behavior Properties")]
    [SerializeField] public bool isTemplate = false;
    [SerializeField] public int minSize = 20;
    [SerializeField] public int maxSize = 360;


    void Start()
    {
        // Get canvas component for drag event
        canvas = transform.gameObject.GetComponentInParent<Canvas>();
        sizeText.text = value.ToString();
        decreaseButton.onClick.AddListener(delegate {ValueUpdate("-"); });
        increaseButton.onClick.AddListener(delegate {ValueUpdate("+"); });
        deleteButton.onClick.AddListener(delegate {  DeleteNode(); });
        decreaseButton.transform.gameObject.SetActive(false);
        
        // Change the colors of the increase and decrease buttons
        decreaseButton.image.color = frameColor;
        increaseButton.image.color = frameColor;

        // Disable things if a template
        if(isTemplate)
        {
            deleteButton.transform.gameObject.SetActive(false);
            decreaseButton.transform.gameObject.SetActive(false);
            increaseButton.transform.gameObject.SetActive(false);
        }
        // Set frame color
        frameImage.color = frameColor;
        iconImage.sprite = iconSprite;
        UpdateNode(true);
    }


    public void ValueUpdate(string op)
    {
        
        if(op == "+")
        {
            value += 5;
            if (value >= maxSize)
            {
                increaseButton.transform.gameObject.SetActive(false);
            }
            if(value - 5 >= minSize)
            {
                decreaseButton.transform.gameObject.SetActive(true);
            }
        }
        else if (op == "-")
        {
            value -= 5;
            if(value <= minSize)
            {
                decreaseButton.transform.gameObject.SetActive(false);
            }
            if(value + 5 == maxSize)
            {
                increaseButton.transform.gameObject.SetActive(true);
            }
        }
        else if (op == "=")
        {
            decreaseButton.transform.gameObject.SetActive(!(value <= minSize));
            increaseButton.transform.gameObject.SetActive(!(value >= maxSize));
        }
        sizeText.text = value.ToString();
        UpdateNode(false);
    }
    
    public void UpdateNode(bool initialized)
    {
        string newName = nodeCommand + " " + value.ToString();
        if (!initialized)
            nodeManager.NodeUpdated(gameObject.name, newName);
        else
            nodeManager.AddNode(newName);
        gameObject.name = newName;

    }

    public void DeleteNode()
    {
        nodeManager.NodeRemoved(gameObject);
        Destroy(gameObject);
    }

} 