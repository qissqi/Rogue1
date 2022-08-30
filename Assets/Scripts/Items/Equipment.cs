using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Equipment : Relic
{

    public override void Reset()
    {
        type = ItemType.Equipment;
    }

    public virtual void Use()
    {

    }


    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (useable && eventData.button == PointerEventData.InputButton.Right)
        {
            Use();
        }
    }

}
