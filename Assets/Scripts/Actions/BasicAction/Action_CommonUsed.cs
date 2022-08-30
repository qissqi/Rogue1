using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 需要手动声明结束
/// </summary>
public class Action_CommonUsed : Actions
{
    System.Action Act;
    Character target;
    string anim;

    public Action_CommonUsed(System.Action act, Character _target = null, string _anim = null)
    {
        Act = act;
        target = _target;
        anim = _anim;
    }

    public override void execute()
    {
        Act();
        if(target!=null && anim!=null)
        {
            target.animator.Play(anim);
        }
    }
}
