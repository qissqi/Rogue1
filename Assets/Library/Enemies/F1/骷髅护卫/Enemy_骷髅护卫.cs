using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_骷髅护卫 : EnemyBase
{
    public bool reburnState = false;
    public EnemySkill Reburn;
    public EnemySkill afterBurn,atk1,atk2;
    public List<EnemySkill> normalSkills = new List<EnemySkill>();
    public override void initialize()
    {
        ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_不死造物(this,Reburn)));
    }

    public override void ReceiveDamage(DamageInfo info)
    {
        base.ReceiveDamage(info);
        if(reburnState)
        {
            animator.Play("Special",0,0f);
        }
    }

    public override void SetCurrentSkill(EnemySkill skill)
    {
        base.SetCurrentSkill(skill);
        if(curSkill == Reburn)
        {
            reburnState = true;
            animator.Play("Special");
        }
    }

    public override void SetCurrentSkill()
    {
        if(curSkill == Reburn)
        {
            curSkill = afterBurn;
        }
        else
        {
            if(curSkill == atk1)
            {
                curSkill = atk2;
            }
            else
            {
                curSkill = normalSkills[Random.Range(0, 3)];
            }
        }
    }

    public override void SetSkillGroup()
    {
        Reburn = new EnemySkill(() =>
        {
            animator.Play("Idle");
            reburnState = false;
            ActionManager.Instance.ActionAddToBottom(new HealAction(this, this, 30));
            ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_力量(this, 1)));
        }, EnemyIntentType.Buff, "复活", this, null);

        afterBurn = new EnemySkill(() =>
        {
            ActionManager.Instance.ActionAddToBottom(new AddBuff(new Buff_力量(BattleInfo.Instance.player, -1)));
            ActionManager.Instance.ActionAddToBottom(new AddBuff(new Weak_Buff(BattleInfo.Instance.player, 1, true)));
        }, EnemyIntentType.Curse, "不死族之咒", this, "Attack1");

        atk1 = new EnemySkill(() =>
        {
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
                new DamageInfo(this, 10)));
            ActionManager.Instance.ActionAddToBottom(new AddBuff(new 易伤_buff(BattleInfo.Instance.player, 1, true)));
        }, EnemyIntentType.AttackDebuff, "重击", this, "Attack1", 10);

        atk2 = new EnemySkill(() =>
        {
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
                new DamageInfo(this, 4)));
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
                new DamageInfo(this, 4)));
        }, EnemyIntentType.Attack, "双刀", this, "Attack1", 4, 2);

        var n1 = new EnemySkill(() =>
        {
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
                new DamageInfo(this, 5)));
            ActionManager.Instance.ActionAddToBottom(new MakeDefend(this, this, 10, true));
        }, EnemyIntentType.AttackDefend, null, this, "Attack1", 5);

        var n2 = new EnemySkill(() =>
        {
            ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
                new DamageInfo(this, 15)));
        }, EnemyIntentType.Attack, null, this, "Attack1", 15);


        normalSkills.Add(n1);
        normalSkills.Add(n2);
        normalSkills.Add(atk1);
    }
}
