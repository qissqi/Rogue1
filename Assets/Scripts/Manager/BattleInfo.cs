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
    public List<EnemyBase> enemies = new List<EnemyBase>(9);
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

    #region ������

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
