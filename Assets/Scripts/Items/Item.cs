using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class Item : ShowExplain,IDragHandler,IBeginDragHandler,IEndDragHandler,IPointerClickHandler
{
    public string itemName;
    public bool inShow;
    private Vector3 itemScale;
    [HideInInspector]public int price = 0;
    public enum ItemType
    {
        Relic,Consumable,Equipment,Other
    }
    [HideInInspector]public int OriginSiblingIndex;
    public int lv;
    public int Lv{
        get => lv;
        set {
            lv=value;
            if(value<1)
            {
                lv=1;
            }
            if(value>5)
            {
                lv=5;
            }
        }
    }

    public ItemType type;
    //public Sprite sprite;
    [HideInInspector] public bool useable;
    [HideInInspector] public bool active;
    public bool Dragable = false;
    [HideInInspector] public Transform OriginParent;

    [HideInInspector] public bool useCallback;
    public delegate void Call(Item item);
    public Call ItemRespone;

    private void Awake()
    {
        itemScale = transform.localScale;
        infoSource = GetIntro;
    }

    private void Start()
    {
        transform.parent.GetComponent<Image>().sprite = InventoryManager.Instance.BasicSprite.sprites[lv];
        ResetInfo();
    }

    public void ResetInfo()
    {
        switch (type)
        {
            case ItemType.Relic:
                explainInfo = "\t遗物：在背包中即生效\n获得不同效果";
                break;
            case ItemType.Consumable:
                explainInfo = "\t消耗品：右键使用（如果可用）\n放置于消耗品栏以在战斗中使用";
                break;
            case ItemType.Equipment:
                explainInfo = "\t装备：右键使用（如果可用）\n放置于装备栏以在战斗中使用";
                break;
            case ItemType.Other:
                break;
            default:
                break;
        }
    }

    public virtual void Add()
    {
        InventoryManager.Instance.AddToInventory(this);
        inShow = false;
        if(type == ItemType.Relic)
        {
            active = true;
        }
    }

    public abstract string GetIntro();

    public virtual void OnBattleLoad() { }

    public virtual void OnBattleEnd() { }

    public virtual void MoveTo(Transform parent)
    {
        transform.SetParent(parent);
        transform.position = parent.position;
        transform.localScale = itemScale;
        parent.GetComponent<Image>().sprite = InventoryManager.Instance.BasicSprite.sprites[lv];
    }


    /// <param name="aimBlock">这个是格子图层的Transform</param>
    /// <returns></returns>
    private bool MoveCheck(Transform aimBlock)
    {
        if((aimBlock.parent?.name == "Equipment" && type != ItemType.Equipment)||
            (aimBlock.parent?.name == "Carry" && type != ItemType.Consumable))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void ExchangePosWith(GameObject _g,bool addToList = true)
    {
        Item _i = _g.GetComponent<Item>();
        //先确定是否对应物品栏(第二层父级)
        //不符合则不移动
        if(!MoveCheck(_g.transform.parent))
        {
            transform.SetParent(OriginParent);
            transform.position = OriginParent.position;
        }
        //移动
        else
        {

            //交换
            if (_i != null)
            {
                //确认交换的物体是否能换过来
                //不符合则不移动
                if (!_i.MoveCheck(OriginParent))
                {
                    transform.SetParent(OriginParent);
                    transform.position = OriginParent.position;
                }
                //成功交换
                else
                {
                    MoveTo(_g.transform.parent);
                    _i.MoveTo(OriginParent);
                    InventoryManager.Instance.EquipmentItem.Remove(_i);
                    InventoryManager.Instance.ConsumablesItem.Remove(_i);
                    if(_i.transform.parent.parent.name == "Equipment")
                    {
                        InventoryManager.Instance.EquipmentItem.Add(_i);
                    }
                    else if(_i.transform.parent.parent.name == "Carry")
                    {
                        InventoryManager.Instance.ConsumablesItem.Add(_i);
                    }
                }
            }
            //仅移动
            else
            {
                MoveTo(_g.transform.parent);

                _g.transform.SetParent(OriginParent);
                _g.transform.position = OriginParent.position;
                //移动需要判断原位置
                OriginParentReset();
            }
        }
        OriginSiblingIndex = transform.parent.GetSiblingIndex();
    }

    /// <summary>
    /// 把原来格子图设回来
    /// </summary>
    public void OriginParentReset()
    {
        if (OriginParent.transform.parent.name == "Carry" || OriginParent.transform.parent.name == "Equipment")
        {
            OriginParent.GetComponent<Image>().sprite = InventoryManager.Instance.BasicSprite.sprites[6];
        }
        else
        {
            OriginParent.GetComponent<Image>().sprite = InventoryManager.Instance.BasicSprite.sprites[0];
        }
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!Dragable)
            return;

        InventoryManager.Instance.EquipmentItem.Remove(this);
        InventoryManager.Instance.ConsumablesItem.Remove(this);
        OriginParent = transform.parent;
        //Vector3 op = transform.position;
        transform.SetParent(transform.parent.parent.parent);
        //transform.position = op;
        //GetComponent<RectTransform>().anchoredPosition = Camera.main.ScreenToWorldPoint(eventData.position);
        transform.position = eventData.pointerCurrentRaycast.worldPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!Dragable)
            return;

        //GetComponent<RectTransform>().anchoredPosition += eventData.delta;
        transform.position = eventData.pointerCurrentRaycast.worldPosition;


        GetComponent<Image>().raycastTarget = false;
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.transform.parent?.parent?.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!Dragable)
            return;

        //transform.SetParent(OriginParent);
        var _g = eventData.pointerCurrentRaycast.gameObject;

        if (_g.CompareTag("Item"))
        {
            ExchangePosWith(_g);
        }
        //不在格子上
        else
        {
            transform.SetParent(OriginParent);
            transform.position = OriginParent.position;
        }

        GetComponent<Image>().raycastTarget = true;
        if(transform.parent.parent.name == "Equipment")
        {
            InventoryManager.Instance.EquipmentItem.Add(this);
        }
        if(transform.parent.parent.name == "Carry")
        {
            InventoryManager.Instance.ConsumablesItem.Add(this);
        }

    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (inShow)
            return;
        if (useCallback && eventData.button == PointerEventData.InputButton.Left)
        {
            ItemRespone(this);
        }
    }
}
