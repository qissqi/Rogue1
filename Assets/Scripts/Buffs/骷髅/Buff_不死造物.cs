using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_不死造物 : Buff
{
    EnemySkill skill;
    EnemyBase enemy;
    public Buff_不死造物(Character _target,EnemySkill _skill) : base(_target,false,true,BuffType.Buff,GetBuffSprite(0))
    {
        num = 30;
        enemy = _target as EnemyBase;
        skill = _skill;
    }

    public override void OnDie()
    {
        owner.HP = 1;
        ActionManager.Instance.ActionAddToHead(new MakeDefend(owner, owner, num, false));
        num -= 10;
        BuffSystem.RefreshBuffUICounter(this);
        enemy.SetCurrentSkill(skill);
        enemy.ShowIntent();
        if(num==0)
        {
            RemoveBuff();
        }
    }

    public override string GetIntro()
    {
        string str = "将死亡时，保留1点血量，获得" + num + "层格挡，\n每次死亡时获得的层数减少10，为0时失效。";
        return str;
    }
}
