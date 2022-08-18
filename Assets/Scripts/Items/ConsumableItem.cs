using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableItem : Item
{
    ConsumableItem()
    {
        type = ItemType.Consumable;
    }

    public virtual void Use()
    {
        OriginParent = transform.parent;
        if (useable)
            MakeEffect();
        var _g = InventoryManager.Instance.EmptyItemPre.transform.GetChild(0).gameObject;
        Instantiate(_g, transform.parent);
        OriginParentReset();
        Destroy(gameObject);
    }

    public abstract void MakeEffect();


}
