using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Resizer: MonoBehaviour
{   
    [Header("References")]
    [SerializeField] GridLayoutGroup[] gridLayouts;
    
    [SerializeField] GameObject compileButton;
    [SerializeField] GameObject toolbox;

    // Start is called before the first frame update
    void Start()
    {
        AdjustScreen();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AdjustScreen()
    {
        float originalHeight = 493;
        float originalWidth = 877;
        int newHeight = Screen.height;
        int newWidth = Screen.width;

       
        float originalSpacing;
        float heightRatio = (float)newHeight / (float)originalHeight;
        float widthRatio = (float)newWidth / (float)originalWidth;
        foreach(GridLayoutGroup grid in gridLayouts)
        {
            originalSpacing = grid.spacing.y;


            grid.cellSize = new Vector2(grid.cellSize.x * widthRatio, grid.cellSize.y * heightRatio);
            grid.spacing = new Vector2(grid.spacing.x * widthRatio, grid.spacing.y * heightRatio);
            grid.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(grid.gameObject.GetComponent<RectTransform>().sizeDelta.x * widthRatio, grid.gameObject.GetComponent<RectTransform>().sizeDelta.y * heightRatio);
        }

        // Compile Button Scaling
        originalHeight = 50;
        originalWidth = 100;
        compileButton.GetComponent<RectTransform>().sizeDelta = new Vector2(compileButton.GetComponent<RectTransform>().sizeDelta.x * widthRatio, compileButton.GetComponent<RectTransform>().sizeDelta.y * heightRatio);

    }
}
