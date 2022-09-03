using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public PlayerInfo()
    {
        Init_SO(GameManager.Instance.allRelics, GameManager.Instance.allEquipment);
        loaded = false;
    }

    private bool loaded = false;
    public int maxHP;
    private int hp;
    public int HP
    {
        set { hp = value > maxHP ? maxHP : value; }
        get => hp;
    }
    public int Golds;
    public int maxCost;
    public void SetProperty(int _maxHP, int _hp, int startGolds, int _maxCost)
    {
        if (!loaded)
        {
            loaded = true;
            maxHP = _maxHP;
            HP = _hp;
            Golds = startGolds;
            maxCost = _maxCost;
        }
        Debug.Log(JsonUtility.ToJson(GameManager.Instance.playerInfo, true));
    }

    public List<List<GameObject>> Relics_Left = new List<List<GameObject>>();
    public List<List<GameObject>> Equipment_Left = new List<List<GameObject>>();
    public List<GameObject> Relics_Get = new List<GameObject>();
    public List<GameObject> Equipment_Get = new List<GameObject>();

    public void Init_SO(SO_SO relics, SO_SO equipment)
    {

        if (relics.objects.Count > 0)
        {
            Relics_Left.Add(new List<GameObject>());
            Relics_Left.Add(new List<GameObject>());
            Relics_Left[0].AddRange(relics.objects[0].gameObjects);
            Relics_Left[1].AddRange(relics.objects[1].gameObjects);
        }
        if (equipment.objects.Count > 0)
        {
            Equipment_Left.Add(new List<GameObject>());
            Equipment_Left.Add(new List<GameObject>());
            Equipment_Left[0].AddRange(equipment.objects[0].gameObjects);
            Equipment_Left[1].AddRange(equipment.objects[1].gameObjects);
        }
    }

    public GameObject GetItem(Item.ItemType type, int a1, int a2)
    {
        GameObject item = null;
        switch (type)
        {
            case Item.ItemType.Relic:
                item = Relics_Left[a1][a2];
                Relics_Left[a1].RemoveAt(a2);
                Relics_Get.Add(item);
                break;

            case Item.ItemType.Equipment:
                item = Equipment_Left[a1][a2];
                Equipment_Left[a1].RemoveAt(a2);
                Equipment_Get.Add(item);
                break;

            default:
                break;
        }
        return item;
    }

    public void NewGetItem(Item item)
    {
        switch (item.type)
        {
            case Item.ItemType.Relic:
                foreach (var item1 in Relics_Left)
                {
                    foreach (var item2 in item1)
                    {
                        if (item2.GetComponent<Item>().itemName == item.itemName)
                        {
                            item1.Remove(item2);
                            Relics_Get.Add(item2);
                            break;
                        }
                    }
                }
                break;

            case Item.ItemType.Equipment:
                foreach (var item1 in Equipment_Left)
                {
                    foreach (var item2 in item1)
                    {
                        if (item2.GetComponent<Item>().itemName == item.itemName)
                        {
                            item1.Remove(item2);
                            Equipment_Get.Add(item2);
                            break;
                        }
                    }
                }
                break;

            default:
                break;
        }
    }

    public void ClearAll()
    {
        loaded = false;
        Relics_Left.Clear();
        Relics_Get.Clear();
        Equipment_Left.Clear();
        Equipment_Left.Clear();

    }

}
