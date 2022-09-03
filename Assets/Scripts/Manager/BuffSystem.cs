using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public static class BuffSystem
{
    public static void AddBuff(Buff _buff)
    {
        Character _target = _buff.owner;
        if (_target == null || _target.dead)
            return;
        //buff�ɵ���
        if(_buff.increasable)
        {
            //����buff�����
            var _b = _target.FindBuff(_buff);
            if (_b != null)
            {
                _b.Counter(_buff.num);
                _target.transform.DOShakePosition(0.6f);
                _b.Combine_GO.transform.DOScale(2f, 0.7f).From().OnComplete(ActionManager.Instance.ActionEnd);
                RefreshBuffUICounter(_b);
                return;
            }
        }

        //buff���ɵ��ӻ�buff�����ڣ�ֱ������
        AddBuffAsNew(_buff, _target);

        
    }

    public static void RefreshBuffUICounter(Buff _buff)
    {
        if (_buff.Combine_GO == null)
            return;
        var t = _buff.Combine_GO.GetComponentInChildren<Text>();
        if(t!=null)
            t.text =_buff.num.ToString();
    }

    public static void AddBuffAsNew(Buff _buff,Character _target)
    {
        Debug.Log("AddBuff:\n" + _buff.GetType());
        //�����ȼ�����buffs
        if (_buff.Prior())
            _target.buffs.Insert(0, _buff);
        else
            _target.buffs.Add(_buff);

        _buff.Effective();

        if (_buff.owner == null)
            return;

        //Icon����ʵ��
        var _g = Object.Instantiate(BattleManager.Instance.BuffBlockPre, _target.BuffArea.transform);
        _g.GetComponent<Image>().sprite = _buff.buffSprite;
        _g.GetComponent<ShowExplain>().infoSource = _buff.GetIntro;
        _buff.Combine_GO = _g;
        var t = _g.GetComponentInChildren<Text>();
        if (_buff.showNum)
        {
            t.text = _buff.num.ToString();
        }
        else
        {
            t.gameObject.SetActive(false);
        }

        //����
        _target.transform.DOShakePosition(0.6f);
        _g.transform.localScale = new Vector3(2, 2);
        _g.transform.DOScale(1, 0.7f);


        //ˢ�����е�����ʾ
        foreach (var _e in BattleInfo.Instance.enemies)
        {
            _e.GetComponent<EnemyBase>().ShowIntent();
        }
        //ˢ�����п�����ʾ
        BattleManager.Instance.AllCardRefresh();
    }

}
