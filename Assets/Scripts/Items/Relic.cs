using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Relic : Item, IBattleActive
{
    public void Reset()
    {
        type = ItemType.Relic;
    }

    public virtual void AfterCardUse(Card card)
    {
    
    }

    public virtual float AtDamageGive(DamageInfo info)
    {
        return info.commonDamage;
    }

    public float AtDamageReceive(DamageInfo info)
    {
        return info.commonDamage;
    }

    public virtual bool Prior()
    {
        return false;
    }
}
