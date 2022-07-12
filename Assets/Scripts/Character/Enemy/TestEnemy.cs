using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestEnemy : EnemyBase
{
    
    bool useDef = false;
    List<EnemySkill> SK1 = new List<EnemySkill>();
    List<EnemySkill> SK2 = new List<EnemySkill>();
    public override void initialize()
    {
        
    }

    public override void ShowIntent()
    {
        curSkill.ShowIntent();
    }

    public override void TakeAction()
    {
        curSkill.TakeSkill();
    }
    public override void SetSkillGroup()
    {
        SK1.Add(new EnemySkill(Skill1, EnemyIntentType.Attack,this,10));
        SK1.Add(new EnemySkill(Skill3, EnemyIntentType.Attack,this,4,2));
        SK2.Add(new EnemySkill(Skill2, EnemyIntentType.Defend,this));
    }

    public override void SetCurrentIntent()
    {
        if(HP<=HPmax/2&&!useDef)
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
        Actions s = new MakeDamage(BattleInfo.Instance.player,
            new DamageInfo(this, ATK + 5));
        ActionManager.Instance.ActionAddToBotton(s);
    }

    public void Skill2()
    {
        ActionManager.Instance.ActionAddToBotton(new MakeDefend(this, 10));
    }

    public void Skill3()
    {
        ActionManager.Instance.ActionAddToBotton(new MakeDamage(BattleInfo.Instance.player,
            new DamageInfo(this, ATK - 1)));
        ActionManager.Instance.ActionAddToBotton(new MakeDamage(BattleInfo.Instance.player,
            new DamageInfo(this, ATK - 1)));
    }

    #endregion


}


