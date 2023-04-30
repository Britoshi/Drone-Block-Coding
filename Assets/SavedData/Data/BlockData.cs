using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockData 
{
    public List<string> blockNames;
    public string programName;


    // When we create a new BlockData, we want to initialize the list of block names
    public BlockData()
    {
        blockNames = new List<string>();
        programName = "My Program";
    }
}
