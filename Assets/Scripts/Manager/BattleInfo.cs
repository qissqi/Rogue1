using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//����ս��ʱ�������յĸ�����Ϣ����ҡ����ˡ���ѡ״̬��
public class BattleInfo : Singleton<BattleInfo>
{

    #region ���
    public Player player;
    public Card ChosenCard;
    public Character ChosenEnemy;
    public Character aim;
    #endregion

    #region ����
    public List<GameObject> enemies = new List<GameObject>();
    #endregion

    private void Update()
    {
        //��һغϣ��Ҽ����¿���
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

    public int enemyNum;

    public void ClearInfo()
    {
        player = null;
        enemies.Clear();

    }


    #region ������

    public int CaculateCardDamage(int damage)
    {
        if (!BattleManager.Instance.battleStart)
            return damage;

        Character target = aim;
        DamageInfo info = new DamageInfo(player, damage,DamageType.Caculate);
        foreach (var _b in player.buffs)
        {
            info.commonDamage = Mathf.FloorToInt(_b.AtDamageGive(info));
        }
        if(target!=null)
        {
            foreach (var _b in target.buffs)
            {
                info.commonDamage = Mathf.FloorToInt(_b.AtDamageReceive(info));
            }
        }
        if (info.commonDamage < 0)
            info.commonDamage = 0;

        return info.commonDamage;
    }

    public int CaculateEnemyDamage(int damage, EnemyBase source)
    {
        DamageInfo info = new DamageInfo(source, damage,DamageType.Caculate);

        foreach (var _b in source.buffs)
        {
            info.commonDamage = Mathf.FloorToInt(_b.AtDamageGive(info));
        }

        foreach (var _b in player.buffs)
        {
            info.commonDamage = Mathf.FloorToInt(_b.AtDamageReceive(info));
        }
        if (info.commonDamage < 0)
            info.commonDamage = 0;
        return info.commonDamage;
    }

    public int CaculateDefendValue(float defend , Character source)
    {
        if (!BattleManager.Instance.battleStart)
            return (int)defend;
        float val = defend;

        foreach (var buff in source.buffs)
        {
            val = buff.OnDefendGiven(val,true);
        }
        if (val < 0)
            val = 0;
        return (int)val;
    }

    #endregion




    #region ��ս��

    //#region ����¼�
    //void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    //{
    //    //���-1���Ҽ�-2
    //    //������
    //    if (eventData.pointerId == -1)  //
    //    {
    //        GameObject _g = eventData.pointerCurrentRaycast.gameObject;
    //        Debug.Log(_g);

    //        if (BattleManager.Instance.CheckPhase(BattlePhase.PlayerTurn))
    //        {
    //            //��������غ�
    //            if (_g.CompareTag("EndButton"))
    //            {
    //                ActionManager.Instance.ActionAddToBotton(new EndPhase());
    //                ChosenCard = null;
    //            }

    //            //�������
    //            else if (_g.CompareTag("Card"))
    //            {
    //                ChosenCard = _g;
    //            }

    //            //�������
    //            else if (_g.CompareTag("Enemy"))
    //            {
    //                //����ָ���ض�����
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

    //            //����հ�
    //            else
    //            {

    //                ChosenCard = null;
    //            }

    //        }

    //    }

    //    //����Ҽ�
    //    if (eventData.pointerId == -2)
    //    {
    //        ChosenCard = null;
    //    }
    //}
    //#endregion
    #endregion


}
