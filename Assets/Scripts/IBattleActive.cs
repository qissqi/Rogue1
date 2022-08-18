using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleActive
{
    public bool Prior();

    /// <summary> 收到伤害前 </summary>
    public float AtDamageReceive(DamageInfo info);

    /// <summary> 伤害给予前 </summary>
    public float AtDamageGive(DamageInfo info);

    public void AfterCardUse(Card card);


}
