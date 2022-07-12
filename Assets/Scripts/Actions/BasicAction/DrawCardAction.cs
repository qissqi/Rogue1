using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawCardAction : Actions
{
    int n;
    int num;
    public int Num { 
        set 
        {
            num = value;
            if(value<=0)
            {
                num = 1;
            }
        } }


    public DrawCardAction(int _num) { Num = _num; }
    public DrawCardAction() { Num = 1; }

    public override void execute()
    {
        BattleManager.Instance.DrawCard(num);
        ActionManager.Instance.DelayActionEnd(0.4f);
    }
}
