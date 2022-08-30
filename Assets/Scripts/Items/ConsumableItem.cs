using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ConsumableItem : Item
{
    public ConsumableItem()
    {
        type = ItemType.Consumable;
    }

    public virtual void Use()
    {
        MakeEffect();
        OriginParent = transform.parent;
        var _g = InventoryManager.Instance.EmptyItemPre.transform.GetChild(0).gameObject;
        Instantiate(_g, transform.parent);
        OriginParentReset();
        GameManager.Instance.CloseExplainBox();
        InventoryManager.Instance.InventoryItems.Remove(this);
        InventoryManager.Instance.ConsumablesItem.Remove(this);
        Destroy(gameObject);
    }

    public abstract void MakeEffect();

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if(useable && eventData.button == PointerEventData.InputButton.Right)
        {
            Use();
        }
    }

}
