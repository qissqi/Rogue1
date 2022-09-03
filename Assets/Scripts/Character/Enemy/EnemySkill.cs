//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

public class EnemySkill 
{
    public EnemyBase enemy;
    public EnemyBase.enemySkillDele SkillDelegate;
    public EnemyIntentType intentType;
    public int basicDamage;
    public int times;
    public string info;
    string anim,skillName;

    public EnemySkill(EnemyBase.enemySkillDele skillD, EnemyIntentType _intentType, string _skillName, EnemyBase sourceEnemy,string _anim,int _damage=-1,int _times=1)
    {
        skillName = _skillName;
        SkillDelegate = skillD;
        intentType = _intentType;
        basicDamage = _damage;
        times = _times;
        enemy = sourceEnemy;
        info = "";
        anim = _anim;
        if(_damage>=0)
        {
            info += _damage;
            if(_times>1)
            {
                info = info + "x" + _times;
            }
        }
    }

    public void TakeSkill()
    {
        if (anim != null)
            ActionManager.Instance.ActionAddToBottom(new Action_CommonUsed(() =>
            {
                enemy.animator.Play(anim);
                if(skillName !=null)
                {
                    BattleUI.Instance.ShowHit(enemy.transform,skillName,UnityEngine.Color.white,0.5f);
                }
            },0.5f));
        SkillDelegate.Invoke();
    }

    public void PlayAnim()
    {

    }

}
