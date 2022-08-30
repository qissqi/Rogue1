using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBase
{
    
    bool useDef = false;
    //public EnemySkill[] SK1 = new EnemySkill[3];
    //public EnemySkill[] SK2 = new EnemySkill[3]; 
    public List<EnemySkill> SK1 = new List<EnemySkill>();
    public List<EnemySkill> SK2 = new List<EnemySkill>();
    public override void initialize()
    {
        
    }


    public override void TakeAction()
    {
        curSkill.TakeSkill();
    }
    public override void SetSkillGroup()
    {
        //SK1[0] = new EnemySkill(Skill1, EnemyIntentType.Attack, this, 10);
        //SK1[1] = new EnemySkill(Skill3, EnemyIntentType.Attack, this, 4, 2);
        //SK2[0] = new EnemySkill(Skill2, EnemyIntentType.Defend, this);

        SK1.Add(new EnemySkill(Skill1, EnemyIntentType.Attack, "",this,null, 10));
        SK1.Add(new EnemySkill(Skill3, EnemyIntentType.Attack, "",this,null, 4, 2));
        SK2.Add(new EnemySkill(Skill2, EnemyIntentType.Defend, "",this,null));
    }

    public override void SetCurrentSkill()
    {
        //SetSkillGroup();
        if(HP<=maxHP/2&&!useDef)
        {
            curSkill = SK2[0];
            useDef = true;
        }
        else
        {
            curSkill = SK1[Random.Range(0, 2)];
        }
    }

    #region ¼¼ÄÜÇø
    public void Skill1()
    {
        animator.Play("Attack1");
        Actions s = new MakeDamage(BattleInfo.Instance.player,
            new DamageInfo(this, 5 + 5));
        ActionManager.Instance.ActionAddToBottom(s);
    }

    public void Skill2()
    {
        ActionManager.Instance.ActionAddToBottom(new MakeDefend(this,this, 10,true));
    }

    public void Skill3()
    {
        animator.Play("Attack1");
        ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
            new DamageInfo(this, 5 - 1)));
        ActionManager.Instance.ActionAddToBottom(new MakeDamage(BattleInfo.Instance.player,
            new DamageInfo(this, 5 - 1)));
    }

    #endregion
    public void en()
    {
        Debug.Log(GO_);
    }

}


