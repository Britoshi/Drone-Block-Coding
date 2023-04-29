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
    [SerializeField] NodeManager nodeManager;

    [Header("Node Properties")]
    // Commands possible: F, B, L, R, U, D, CC, C
    [SerializeField] string nodeCommand;
    [SerializeField] TextMeshProUGUI sizeText;
    [SerializeField] public Button increaseButton;
    [SerializeField] public Button decreaseButton;
    [SerializeField] public Button deleteButton;

    [Header("Image Properties")]
    [SerializeField] Image frameImage;
    [SerializeField] Image iconImage;
    [SerializeField] Sprite iconSprite;
    // Possilbe frame colors:
    /*
     #b27b14
     #3e2e4f
     #843021
     #011434
    */
    [SerializeField] Color frameColor;
    
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
        deleteButton.onClick.AddListener(delegate { Destroy(this.gameObject); });
        decreaseButton.transform.gameObject.SetActive(false);

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
        UpdateNode();
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
        else
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
        sizeText.text = value.ToString();
        UpdateNode();
    }
    
    public void UpdateNode()
    {
        gameObject.name = nodeCommand + " " + value.ToString();
    }
} 