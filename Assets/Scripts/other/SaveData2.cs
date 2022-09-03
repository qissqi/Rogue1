using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData2
{
    #region Basic
    public float BGMvolume, SEvolume;
    public bool firstShop, Tutorial;

    #endregion

    [TextArea]
    public List<string> ItemInfo;

    [TextArea]
    public string playerInfo;
}
