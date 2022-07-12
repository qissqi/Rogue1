using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//管理战斗时所需掌握的各种信息，玩家、敌人、待选状态等
public class BattleInfo : Singleton<BattleInfo>
{

    #region 玩家
    public Player player;
    public Card ChosenCard;
    public Character ChosenEnemy;
    public Character aim;
    #endregion

    #region 敌人
    public List<EnemyBase> enemies = new List<EnemyBase>(9);
    #endregion

    private void Update()
    {
        //玩家回合，右键放下卡牌
        if (BattleManager.Instance.CheckPhase(BattlePhase.PlayerTurn))
        {
            if (Input.GetMouseButtonDown(1) && ChosenCard!=null)
            {
                ChosenCard.Drop();
                ChosenCard = null;
                aim = null;
                BattleManager.Instance.AllCardRefresh();
            }
        }

    }

    #region 计算器

    public int CaculateCardDamage(int damage)
    {
        Character target = aim;
        DamageInfo info = new DamageInfo(player, damage);
        foreach (var _b in player.buffs)
        {
            info.commonDamage = (int)_b.AtDamageGive(info);
        }
        if(target!=null)
        {
            foreach (var _b in target.buffs)
            {
                info.commonDamage = (int)_b.AtDamageReceive(info);
            }
        }

        return info.commonDamage;
    }

    public int CaculateEnemyDamage(int damage, EnemyBase source)
    {
        DamageInfo info = new DamageInfo(source, damage);

        foreach (var _b in source.buffs)
        {
            info.commonDamage = (int)_b.AtDamageGive(info);
        }

        foreach (var _b in player.buffs)
        {
            info.commonDamage = (int)_b.AtDamageReceive(info);
        }

        return info.commonDamage;
    }

    #endregion



    #region 古战场

    //#region 点击事件
    //void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    //{
    //    //左键-1，右键-2
    //    //点击左键
    //    if (eventData.pointerId == -1)  //
    //    {
    //        GameObject _g = eventData.pointerCurrentRaycast.gameObject;
    //        Debug.Log(_g);

    //        if (BattleManager.Instance.CheckPhase(BattlePhase.PlayerTurn))
    //        {
    //            //点击结束回合
    //            if (_g.CompareTag("EndButton"))
    //            {
    //                ActionManager.Instance.ActionAddToBotton(new EndPhase());
    //                ChosenCard = null;
    //            }

    //            //点击卡牌
    //            else if (_g.CompareTag("Card"))
    //            {
    //                ChosenCard = _g;
    //            }

    //            //点击敌人
    //            else if (_g.CompareTag("Enemy"))
    //            {
    //                //卡牌指向特定人物
    //                if (ChosenCard !=null&&(
    //                    ChosenCard.GetComponent<Card>().target == Card.Target.Enemy ||
    //                    ChosenCard.GetComponent<Card>().target == Card.Target.PlayerNEnemy))
    //                {
    //                    if (ChosenCard != null)
    //                    {
    //                        ChosenEnemy = _g;
    //                        //BattleManager.ActionManager.ActionAddToBotton(ChosenCard.GetComponent<Card>().UseCard);
    //                        ChosenCard.GetComponent<Card>().UseCard();
    //                        ChosenCard = null;
    //                    }
    //                }
    //            }

    //            //点击空白
    //            else
    //            {

    //                ChosenCard = null;
    //            }

    //        }

    //    }

    //    //点击右键
    //    if (eventData.pointerId == -2)
    //    {
    //        ChosenCard = null;
    //    }
    //}
    //#endregion
    #endregion


}
