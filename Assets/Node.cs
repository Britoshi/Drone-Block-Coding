using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class Node : MonoBehaviour
{
    // Variables to phase out 
    TextMeshProUGUI size_text; 
    Button increase;
    Button decrease;

    // Variables to keep
    int value = 20;  
    Canvas canvas;

    [Header("Image Properties")]
    [SerializeField] Image frameImage;
    // Possilbe frame colors:
    /*
     #b27b14
     #3e2e4f
     #843021
     #011434
    */
    [SerializeField] Color frameColor;

    void Start()
    {
        // Get canvas component for drag event
        canvas = transform.gameObject.GetComponentInParent<Canvas>();


        // We'll eventually get rid of this
        foreach(Transform child in transform)
        {
            Debug.Log(child.gameObject.tag);
            if(child.gameObject.tag == "Label")
            {
                size_text = child.GetComponent<TextMeshProUGUI>();
                size_text.text = value.ToString();
            }
            if(child.gameObject.name == "Increase")
            {
                increase = child.GetComponent<Button>();
                increase.onClick.AddListener(delegate { ValueUpdate("+"); });
            }
            if(child.gameObject.name == "Decrease")
            {
                decrease = child.GetComponent<Button>();
                decrease.onClick.AddListener(delegate { ValueUpdate("-"); });
            }

        }
        decrease.transform.gameObject.SetActive(false);

        // Set frame color
        frameImage.color = frameColor;
        UpdateNode();
    }

    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerData = (PointerEventData)data;
        Vector2 localPointerPosition;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, pointerData.position, pointerData.pressEventCamera, out localPointerPosition))
        {
            transform.position = canvas.transform.TransformPoint(localPointerPosition);
        }
    }

    public void ValueUpdate(string op)
    {
        
        if(op == "+")
        {
            value += 5;
            if (value == 360)
            {
                increase.transform.gameObject.SetActive(false);
            }
            if(value - 5 == 20)
            {
                decrease.transform.gameObject.SetActive(true);
            }
        }
        else
        {
            value -= 5;
            if(value == 20)
            {
                decrease.transform.gameObject.SetActive(false);
            }
            if(value + 5 == 360)
            {
                increase.transform.gameObject.SetActive(true);
            }
        }
        size_text.text = value.ToString();
        UpdateNode();
    }
    
    public void UpdateNode()
    {
        Debug.Log("Haha I don't update anymore :)");
        //dropdown.transform.parent.gameObject.transform.gameObject.name = dropdown.options[dropdown.value].text[0] + " " + value.ToString();
    }
}
