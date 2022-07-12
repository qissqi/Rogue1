using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddBuff : Actions
{
    private Buff buff;
    public AddBuff(Buff _buff)
    {
        buff = _buff;
    }
    public override void execute()
    {
        BuffSystem.AddBuff(buff);
    }
}
