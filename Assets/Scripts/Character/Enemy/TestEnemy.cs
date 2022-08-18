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

    public override void ShowIntent(bool showNumber = true)
    {
        //SetSkillGroup();
        IntentText.text = "";
        UnityEngine.UI.Image image = IntentImage;
        SpriteList _sp = IntentList;
        switch (curSkill.intentType)
        {
            case EnemyIntentType.Unknown:
                image.sprite = _sp.sprites[15];
                break;

            case EnemyIntentType.Attack:
                image.sprite = _sp.sprites[0];
                var _d = BattleInfo.Instance.CaculateEnemyDamage(curSkill.basicDamage,this);
                IntentText.text = _d.ToString();
                if (curSkill.times > 1)
                {
                    IntentText.text += "x" + curSkill.times;
                }
                break;

            case EnemyIntentType.Defend:
                image.sprite = _sp.sprites[7];
                break;

            case EnemyIntentType.Buff:
                image.sprite = _sp.sprites[4];
                break;

            case EnemyIntentType.Debuff:
                image.sprite = _sp.sprites[5];
                break;

            case EnemyIntentType.Curse:
                image.sprite = _sp.sprites[6];
                break;

            default:
                image.sprite = _sp.sprites[15];
                break;
        }
        image.SetNativeSize();
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

        SK1.Add(new EnemySkill(Skill1, EnemyIntentType.Attack, this, 10));
        SK1.Add(new EnemySkill(Skill3, EnemyIntentType.Attack, this, 4, 2));
        SK2.Add(new EnemySkill(Skill2, EnemyIntentType.Defend, this));
    }

    public override void SetCurrentIntent()
    {
        //SetSkillGroup();
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
    public void en()
    {
        Debug.Log(GO_);
    }

}


