using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "myItem/saveData",fileName ="saveData")]
public class SaveData : ScriptableObject
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
