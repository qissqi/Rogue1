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
        if (_target == null)
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

        ///
        ///var n = new GameObject(_buff.ToString());
        ///var _g = GameObject.Instantiate(n, _target.BuffArea.transform);
        ///Object.Destroy(n);
        ///_g.AddComponent<Image>().sprite = _buff.buffSprite;
        ///_g.AddComponent<ShowExplain>().explainInfo = _buff.GetIntro();
        ///_buff.Combine_GO = _g;
        ///if(_buff.showNum)
        ///{
        ///    var m = new GameObject("Counter");
        ///    var _t = GameObject.Instantiate(m, _g.transform);
        ///    Object.Destroy(m);
        ///    _t.transform.localScale = new Vector3(0.2f, 0.2f);
        ///    var text = _t.AddComponent<Text>();
        ///    text.alignment = TextAnchor.LowerRight;
        ///    text.font = BattleUI.Instance.Font;
        ///    text.color = Color.black;
        ///    text.fontSize = 55;
        ///    text.text = _buff.num.ToString(); 
        ///}
        ///

        
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
        _g.transform.DOScale(2f, 0.7f).From();


        //ˢ�����е�����ʾ
        foreach (var _e in BattleInfo.Instance.enemies)
        {
            _e.GetComponent<EnemyBase>().ShowIntent();
        }
        //ˢ�����п�����ʾ
        BattleManager.Instance.AllCardRefresh();
    }

}
