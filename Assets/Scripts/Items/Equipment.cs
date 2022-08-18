using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item,IBattleActive
{
    public virtual void AfterCardUse(Card card)
    {
        
    }

    public virtual float AtDamageGive(DamageInfo info)
    {
        return info.commonDamage;
    }

    public virtual float AtDamageReceive(DamageInfo info)
    {
        return info.commonDamage;
    }

    public virtual bool Prior()
    {
        return false;
    }

    private void Reset()
    {
        type = ItemType.Equipment;
    }

}
