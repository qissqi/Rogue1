using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealAction : Actions
{
    Character target,source;
    int amount;
    string anim;
    public HealAction(Character _source, Character _target,int _amount,string anim = null)
    {
        target = _target;
        source = _source;
        amount = _amount;
    }

    public override void execute()
    {
        target.HP += amount;
        BattleUI.Instance.ShowNumber(target.transform, amount, Color.green);
        if(anim!=null)
        {
            source.animator.Play(anim);
        }
        else
        {
            ActionManager.Instance.DelayActionEnd(0.3f);
        }
    }
}
