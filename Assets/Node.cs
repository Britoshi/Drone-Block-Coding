using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Node : MonoBehaviour
{
    // Variables to phase out 
    Button increase;
    Button decrease;

    // Variables to keep
    int value = 20;  
    Canvas canvas;

    [Header("Outside Scripts")]
    [SerializeField] NodeManager nodeManager;

    [Header("Node Properties")]
    // Commands possible: F, B, L, R, U, D, CC, C
    [SerializeField] string nodeCommand;
    [SerializeField] TextMeshProUGUI sizeText;
    [SerializeField] Button increaseButton;
    [SerializeField] Button decreaseButton;
    [SerializeField] GameObject placeholderPrefab;

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
    [SerializeField] public bool isDraggable = true;

    public int index = -1;
    GameObject placeholder = null;
    bool isDragging = false;

    void Start()
    {
        // Get canvas component for drag event
        canvas = transform.gameObject.GetComponentInParent<Canvas>();
        sizeText.text = value.ToString();
        decreaseButton.onClick.AddListener(delegate {ValueUpdate("-"); });
        increaseButton.onClick.AddListener(delegate {ValueUpdate("+"); });
        decreaseButton.transform.gameObject.SetActive(false);

        // Set frame color
        frameImage.color = frameColor;
        iconImage.sprite = iconSprite;
        UpdateNode();
    }

    public void DragHandler(BaseEventData data)
    {
        if(!isDraggable) return;
        if(!isDragging)
        {
            placeholder = Instantiate(placeholderPrefab, transform.parent);
            placeholder.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
            isDragging = true;
        }
        PointerEventData pointerData = (PointerEventData)data;
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pointerData.position, pointerData.pressEventCamera, out localPointerPosition))
        {
            transform.position = canvas.transform.TransformPoint(localPointerPosition);
            nodeManager.ProgramNodeMovement(placeholder, pointerData.position);
        }
    }

    public void DragEndHandler(BaseEventData data)
    {
        if(!isDraggable) return;
        int newIndex = placeholder.transform.GetSiblingIndex() - 1;
        placeholder.transform.SetParent(transform.parent.parent);
        Destroy(placeholder);
        isDragging = false;
        nodeManager.UpdateNodePositions(index, newIndex);
    }

    public void ClickHandler(BaseEventData data)
    {
        if(isDraggable) return;
        nodeManager.CreateNode(this.gameObject);
    }

    public void ValueUpdate(string op)
    {
        
        if(op == "+")
        {
            value += 5;
            if (value == 360)
            {
                increaseButton.transform.gameObject.SetActive(false);
            }
            if(value - 5 == 20)
            {
                decreaseButton.transform.gameObject.SetActive(true);
            }
        }
        else
        {
            value -= 5;
            if(value == 20)
            {
                decreaseButton.transform.gameObject.SetActive(false);
            }
            if(value + 5 == 360)
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
