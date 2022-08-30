using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBuff : Actions
{
    private Buff buff;
    Character source;
    string anim;

    public AddBuff(Buff _buff,Character _source = null,string _anim = null)
    {
        buff = _buff;
        source = _source;
        anim = _anim;
    }
    public override void execute()
    {
        BuffSystem.AddBuff(buff);
        if(anim!=null)
        {
            source.animator.Play(anim);
        }
        else
        {
            ActionManager.Instance.DelayActionEnd(0.5f);
        }
    }
}
