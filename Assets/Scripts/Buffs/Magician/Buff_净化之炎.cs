using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_净化之炎 : Buff
{
    public static new string intro = "净化之炎：\n灼烧将造成等同于其层数的伤害。";

    public Buff_净化之炎(Character _target) : base(_target,true,false,BuffType.Buff, BattleUI.Instance.BuffSprites.sprites[0])
    {
        num = 1;
    }

    public override void Counter(int _num)
    {
        
    }

    public override string GetIntro()
    {
        return intro;
    }
}
