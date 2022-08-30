using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "myItem/saveData",fileName ="saveData")]
public class SaveData : ScriptableObject
{
    public List<GameObject> Relics_Get;
    public List<List<GameObject>> Relics_Left;

    public List<GameObject> Equipments_Get;
    public List<List<GameObject>> Equipments_Left;

    [TextArea]
    public List<string> ItemInfo;

    [TextArea]
    public string playerInfo;

}
