using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resizer: MonoBehaviour
{   
    [Header("References")]
    [SerializeField] GridLayoutGroup[] gridLayouts;
    
    [SerializeField] GameObject[] buttons;

    [Header("Settings")]
    [SerializeField] float originalHeight = 493f;
    [SerializeField] float originalWidth = 877f;
    [SerializeField] bool showScreenSize = false;

    // Start is called before the first frame update
    void Awake()
    {
        if(showScreenSize)
        {
            print($"[Resizer] Screen Size: ({Screen.width}, {Screen.height})");
            originalHeight = Screen.height;
            originalWidth = Screen.width;
        }
        AdjustScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AdjustScreen()
    {
        int newHeight = Screen.height;
        int newWidth = Screen.width;

       
        float originalSpacing;
        float heightRatio = (float)newHeight / (float)originalHeight;
        float widthRatio = (float)newWidth / (float)originalWidth;
        Debug.Log($"[Resizer] Height Ratio: {heightRatio} | Width Ratio: {widthRatio}");
        foreach(GridLayoutGroup grid in gridLayouts)
        {
            originalSpacing = grid.spacing.y;


            grid.cellSize = new Vector2(grid.cellSize.x * widthRatio, grid.cellSize.y * heightRatio);
            grid.spacing = new Vector2(grid.spacing.x * widthRatio, grid.spacing.y * heightRatio);
            grid.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(grid.gameObject.GetComponent<RectTransform>().sizeDelta.x * widthRatio, grid.gameObject.GetComponent<RectTransform>().sizeDelta.y * heightRatio);
        }

        if(buttons != null)
        {
            // Compile Button Scaling
            originalHeight = 50;
            originalWidth = 100;
            foreach(GameObject button in buttons)
                button.GetComponent<RectTransform>().sizeDelta = new Vector2(button.GetComponent<RectTransform>().sizeDelta.x * widthRatio, button.GetComponent<RectTransform>().sizeDelta.y * heightRatio);
        }
    }
}
