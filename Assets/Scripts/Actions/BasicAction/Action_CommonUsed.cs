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
    float time;

    public Action_CommonUsed(System.Action act, float _time, Character _target = null, string _anim = null)
    {
        Act = act;
        target = _target;
        anim = _anim;
        time = _time;
    }

    public override void execute()
    {
        Act();
        if(target!=null && anim!=null)
        {
            target.animator.Play(anim);
        }
        else
        {
            ActionManager.Instance.DelayActionEnd(time);
        }
    }
}
