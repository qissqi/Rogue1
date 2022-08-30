using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattleActive
{
    public bool Prior();

    /// <summary> �յ��˺�ǰ </summary>
    public float AtDamageReceive(DamageInfo info);

    /// <summary> �˺�����ǰ </summary>
    public float AtDamageGive(DamageInfo info);

    public float OnDefendGiven(float defend,bool caculate = false);

    public void AfterCardUse(Card card);

    public abstract void OnDie();



}