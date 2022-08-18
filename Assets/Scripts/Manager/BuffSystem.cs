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

        //buff�ɵ���
        if(_buff.increasable)
        {
            //�����Ƿ����д�Buff
            foreach (var __b in _target.buffs)
            {
                var _b = __b as Buff;
                //����buff�����
                if(_b.GetType()==_buff.GetType())
                {
                    _b.Counter(_buff.num);
                    _target.transform.DOShakePosition(0.6f);
                    _b.Combine_GO.transform.DOScale(2f, 0.7f).From().OnComplete(ActionManager.Instance.ActionEnd);
                    //����return��
                    return;
                }
            }
        }
        //buff���ɵ��ӻ�buff�����ڣ�ֱ������
        Debug.Log("AddBuff:\n" + _buff.GetType());
        //�����ȼ�����buffs
        if (_buff.Prior())
            _target.buffs.Insert(0, _buff);
        else
            _target.buffs.Add(_buff);

        _buff.Effective();
        
        //UI����ʵ��
        var _g = GameObject.Instantiate(new GameObject(_buff.ToString()), _target.BuffArea.transform);
        _g.AddComponent<Image>().sprite = _buff.buffSprite;
        _g.AddComponent<ShowExplain>().explainInfo = _buff.GetIntro();
        _buff.Combine_GO = _g;

        if(_buff.increasable)
        {
            var _t = GameObject.Instantiate(new GameObject("Counter"), _g.transform);
            _t.transform.localScale = new Vector3(0.2f, 0.2f);
            var text = _t.AddComponent<Text>();
            text.alignment = TextAnchor.LowerRight;
            text.font = BattleUI.Instance.Font;
            text.color = Color.white;
            text.fontSize = 55;
            text.text = _buff.num.ToString();
        }

        //����
        _target.transform.DOShakePosition(0.6f);
        _g.transform.DOScale(2f, 0.7f).From().OnComplete(ActionManager.Instance.ActionEnd);
        

        //ˢ�����е�����ʾ
        foreach (var _e in BattleInfo.Instance.enemies)
        {
            _e.GetComponent<EnemyBase>().ShowIntent();
        }
        //ˢ�����п�����ʾ
        BattleManager.Instance.AllCardRefresh();
    }

    public static void RefreshBuffUICounter(Buff _buff)
    {
        _buff.Combine_GO.GetComponentInChildren<Text>().text = _buff.num.ToString();
    }


}
