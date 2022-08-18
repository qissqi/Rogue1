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

    public EnemySkill(EnemyBase.enemySkillDele skillD, EnemyIntentType _intentType, EnemyBase sourceEnemy,int _damage=-1,int _times=1)
    {
        SkillDelegate = skillD;
        intentType = _intentType;
        basicDamage = _damage;
        times = _times;
        enemy = sourceEnemy;
        info = "";
        if(_damage>=0)
        {
            info += _damage;
            if(_times>1)
            {
                info = info + "x" + _times;
            }
        }
    }
    //public void ShowIntent(bool showNumber=true)
    //{
    //    enemy.IntentText.text = "";
    //    UnityEngine.UI.Image image = enemy.IntentImage;
    //    SpriteList _sp = enemy.IntentList;
    //    switch (intentType)
    //    {
    //        case EnemyIntentType.Unknown:
    //            image.sprite = _sp.sprites[15];
    //            break;

    //        case EnemyIntentType.Attack:
    //            image.sprite = _sp.sprites[0];
    //            var _d = BattleInfo.Instance.CaculateEnemyDamage(basicDamage, enemy);
    //            enemy.IntentText.text = _d.ToString();
    //            if(times>1)
    //            {
    //                enemy.IntentText.text += "x" + times;
    //            }
    //            break;

    //        case EnemyIntentType.Defend:
    //            image.sprite = _sp.sprites[7];
    //            break;

    //        case EnemyIntentType.Buff:
    //            image.sprite = _sp.sprites[4];
    //            break;

    //        case EnemyIntentType.Debuff:
    //            image.sprite = _sp.sprites[5];
    //            break;

    //        case EnemyIntentType.Curse:
    //            image.sprite = _sp.sprites[6];
    //            break;

    //        default:
    //            image.sprite = _sp.sprites[15];
    //            break;
    //    }
    //    image.SetNativeSize();
    //}

    public void TakeSkill()
    {
        SkillDelegate.Invoke();
    }

}
