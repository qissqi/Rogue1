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
            animator.Play(atk_anim);
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
                                                new DamageInfo(this, 10)));
        }, EnemyIntentType.Attack, null,this, null,10);

        EnemySkill atk2 = new EnemySkill(() =>
        {
            animator.Play(atk_anim);
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
                new DamageInfo(this, 5)));
            ActionManager.Instance.ActionAddToBottom(new AddBuff(new Weak_Buff(BattleInfo.Instance.player, 1, true)));
        }, EnemyIntentType.AttackDebuff, "ճҺ����",this, null,5);

        EnemySkill buff1 = new EnemySkill(() =>
        {
            animator.Play(atk_anim);
            ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_����(this, 1)));
        }, EnemyIntentType.Buff, "ǿ��", this, null);

        EnemySkill defend1 = new EnemySkill(() =>
        {
            animator.Play(atk_anim);
            ActionManager.Instance.ActionAddToBottom(new MakeDefend(this, this, 10, true));
        }, EnemyIntentType.Defend, null,this, null);

        Skills.Add(atk1);
        Skills.Add(atk2);
        Skills.Add(buff1);
        Skills.Add(defend1);


    }

}
