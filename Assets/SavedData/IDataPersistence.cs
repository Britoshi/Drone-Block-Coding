using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    void LoadProgram(BlockData data);

    void SaveProgram(ref BlockData data);

}
