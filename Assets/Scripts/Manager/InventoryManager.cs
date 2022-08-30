using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class InventoryManager : Singleton<InventoryManager>
{
    public GameObject InventoryPanel;
    public GameObject EquipmentPanel;
    public GameObject CarryPanel;

    public GameObject EmptyItemPre;
    public SpriteList BasicSprite;

    public Text pageText;

    public int maxPage = 1;
    public int page = 1;

    /// <summary> 背包中的所有物品 </summary>
    public List<Item> InventoryItems = new List<Item>();

    public List<Item> ConsumablesItem = new List<Item>();
    public List<Item> EquipmentItem = new List<Item>();


    private void OnEnable()
    {
        FreshMaxPage();
        GameManager.Instance.currentscene = GameManager.GameScene.Inventory;
    }

    private void OnDisable()
    {
        GameManager.Instance.currentscene = GameManager.GameScene.Map;
        InventoryPanel.GetComponent<GridLayoutGroup>().padding.top = 6;
        page = 1;
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        pageText.text = page + "/" + maxPage;
        if(InventoryPanel.transform.childCount/16 > maxPage)
        {
            DeletePage(maxPage + 1);
        }
        else if(InventoryPanel.transform.childCount/16 < maxPage)
        {
            AddPage();
        }
    }

    public void AddToInventory(Item _item)
    {
        _item.Dragable = true;
        //去掉原空Item
        var _block = SearchEmptyBlock(true).transform;
        Destroy(_block.transform.GetChild(0).gameObject);
        //var _newitem = Instantiate(_item.gameObject, _block.transform).GetComponent<Item>();
        _item.MoveTo(_block);
        _block.GetComponent<Image>().sprite = BasicSprite.sprites[_item.Lv];

        InventoryItems.Add(_item);
        //if(_item.type == Item.ItemType.Relic)
        //{
        //    GameManager.Instance.currentCharacter.ActiveItem.Add(_item);
        //}
        FreshMaxPage();
    }


    public void SortInventory()
    {
        int index=0;
        Transform _IV = InventoryPanel.transform;
        
        //从遗物开始
        foreach (var _item in InventoryItems)
        {
            if(_item.type == Item.ItemType.Relic)
            {
                _item.OriginParent = _item.transform.parent;
                _item.ExchangePosWith(_IV.GetChild(index).GetChild(0).gameObject);
                index++;
            }
        }

        //消耗品
        foreach (var _item in InventoryItems)
        {
            if(_item.type== Item.ItemType.Consumable && !ConsumablesItem.Contains(_item))
            {
                _item.OriginParent = _item.transform.parent;
                _item.ExchangePosWith(_IV.GetChild(index).GetChild(0).gameObject);
                index++;
            }
        }

        //装备
        foreach (var _item in InventoryItems)
        {
            if(_item.type== Item.ItemType.Equipment && !EquipmentItem.Contains(_item))
            {
                _item.OriginParent = _item.transform.parent;
                _item.ExchangePosWith(_IV.GetChild(index).GetChild(0).gameObject);
                index++;
            }
        }

        //删除多余页数
        FreshMaxPage();
    }

    public void FreshMaxPage()
    {
        maxPage = 2 + InventoryItems.Count / 16;
        if (InventoryItems.Count % 16 == 0)
            maxPage -= 1;
    }

    public void AddPage()
    {
        for (int i = 0; i < 16; i++)
        {
            var _n = Instantiate(EmptyItemPre, InventoryPanel.transform);
        }
    }

    public void DeletePage(int _page)
    {
        int n1 = 16 * (_page - 1);
        for (int i = 0; i < 16; i++)
        {
            Destroy(InventoryPanel.transform.GetChild(n1));
        }

    }

    public void NextPage()
    {
        if(page<maxPage)
        {
            page++;
            InventoryPanel.GetComponent<GridLayoutGroup>().padding.top -= 380;
            InventoryPanel.GetComponent<GridLayoutGroup>().enabled = false;
            InventoryPanel.GetComponent<GridLayoutGroup>().enabled = true;
        }
    }

    public void LastPage()
    {
        if(page>1)
        {
            page--;
            InventoryPanel.GetComponent<GridLayoutGroup>().padding.top += 380;
            InventoryPanel.GetComponent<GridLayoutGroup>().enabled = false;
            InventoryPanel.GetComponent<GridLayoutGroup>().enabled = true;
        }
    }

    public GameObject SearchEmptyBlock(bool addNewPage)
    {
        GameObject _block = null;
        for (int i = 0; i < InventoryPanel.transform.childCount; i++)
        {
            var _b= InventoryPanel.transform.GetChild(i).gameObject;
            if(_b.transform.GetChild(0).GetComponent<Item>() ==null)
            {
                return _b;
            }
        }

        //无空格，加页？
        if(addNewPage)
        {
            for (int i = 0; i < 16; i++)
            {
                var _n = Instantiate(EmptyItemPre,InventoryPanel.transform);
                if(i==0)
                {
                    _block = _n;
                }
            }
            maxPage++;
        }

        return _block;

    }


}
