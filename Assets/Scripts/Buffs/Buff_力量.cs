using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_力量 : Buff
{
    public static new string intro =
        "力量：\n攻击造成的伤害增加对应层数的值。";

    public Buff_力量(Character _target,int _amount) : base(_target,true,true,_amount>0?BuffType.Buff:BuffType.Debuff,
        BattleUI.Instance.BuffSprites.sprites[0])
    {
        num = _amount;

    }


    public override bool Prior()
    {
        return true;
    }

    public override float AtDamageGive(DamageInfo info)
    {
        var d = info.commonDamage + num;
        return d > 0 ? d : 0;
    }

    public override void Counter(int _num)
    {
        num += _num;
        if (num == 0)
        {
            RemoveBuff();
        }
        else
        {
            if(num>0)
            {
                buffType = BuffType.Buff;
            }
            else
            {
                buffType = BuffType.Debuff;
            }
            BuffSystem.RefreshBuffUICounter(this);
        }
    }

    public override string GetIntro()
    {
        var a = num > 0 ? "增加" : "减少";
        var s = "力量：\n攻击造成的伤害" + a + Mathf.Abs(num) + "点。";
        return s;
    }
}
