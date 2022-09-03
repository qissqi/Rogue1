using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_敏捷 : Buff
{

    public static new string intro =
        "敏捷：\n获得的格挡增加对应层数的值。";
    public Buff_敏捷(Character _target,int amount):base(_target,true,true,amount>0?BuffType.Buff:BuffType.Debuff,
        GetBuffSprite("敏捷"))
    {
        num = amount;
    }

    public override bool Prior()
    {
        return true;
    }

    public override float OnDefendGiven(float defend, bool caculate = false)
    {
        var d = defend + num;
        return d>0?d:0;
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
            if (num > 0)
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
        var s = "敏捷：\n获得的格挡"+ a + Mathf.Abs(num) + "点。";
        return s;
    }
}
