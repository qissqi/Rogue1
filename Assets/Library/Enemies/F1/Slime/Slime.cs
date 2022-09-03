using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : EnemyBase
{
    List<EnemySkill> Skills = new List<EnemySkill>();
    bool defend = false;
    public override void initialize()
    {
        
    }

    public override void SetCurrentSkill()
    {
        if(hp>maxHP/2)
        {
            curSkill = Skills[Random.Range(0, 2)];
        }
        else
        {
            if(!defend)
            {
                defend = true;
                curSkill = Skills[3];
            }
            else
            {
                curSkill = Skills[Random.Range(0, 4)];
            }
        }
    }

    public override void SetSkillGroup()
    {
        string atk_anim = "Attack1";
        EnemySkill atk1 = new EnemySkill(() =>
        {
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
                                                new DamageInfo(this, 8)));
        }, EnemyIntentType.Attack, null,this, atk_anim,8);

        EnemySkill atk2 = new EnemySkill(() =>
        {
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
                new DamageInfo(this, 4)));
            ActionManager.Instance.ActionAddToBottom(new AddBuff(new Weak_Buff(BattleInfo.Instance.player, 1, true)));
        }, EnemyIntentType.AttackDebuff, "粘液喷吐",this, atk_anim, 4);

        EnemySkill buff1 = new EnemySkill(() =>
        {
            ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_力量(this, 1)));
        }, EnemyIntentType.Buff, "强化", this, atk_anim);

        EnemySkill defend1 = new EnemySkill(() =>
        {
            ActionManager.Instance.ActionAddToBottom(new MakeDefend(this, this, 8, true));
        }, EnemyIntentType.Defend, null,this, atk_anim);

        Skills.Add(atk1);
        Skills.Add(atk2);
        Skills.Add(buff1);
        Skills.Add(defend1);


    }

}
