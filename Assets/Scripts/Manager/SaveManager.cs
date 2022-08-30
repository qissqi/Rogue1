using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : Singleton<SaveManager>
{
    public bool HasSave;
    public SaveData saveData;


    public GO_List GO;
    public GameObject test;


    public void ClearAll()
    {
        saveData.playerInfo= "";
        saveData.ItemInfo.Clear();

        saveData.Relics_Get.Clear();
        saveData.Relics_Left.Clear();
        saveData.Relics_Left = new List<List<GameObject>>(2);
        saveData.Relics_Left[0].AddRange(GameManager.Instance.allRelics.objects[0].gameObjects);
        saveData.Relics_Left[1].AddRange(GameManager.Instance.allRelics.objects[1].gameObjects);

        saveData.Equipments_Get.Clear();
        saveData.Equipments_Left.Clear();
        saveData.Equipments_Left = new List<List<GameObject>>(2);
        saveData.Equipments_Left[0].AddRange(GameManager.Instance.allEquipment.objects[0].gameObjects);
        saveData.Equipments_Left[1].AddRange(GameManager.Instance.allEquipment.objects[1].gameObjects);
    }

}
