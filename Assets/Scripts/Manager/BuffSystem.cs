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

        //buff可叠加
        if(_buff.increasable)
        {
            //查找是否已有此Buff
            foreach (var __b in _target.buffs)
            {
                var _b = __b as Buff;
                //已有buff则叠加
                if(_b.GetType()==_buff.GetType())
                {
                    _b.Counter(_buff.num);
                    _target.transform.DOShakePosition(0.6f);
                    _b.Combine_GO.transform.DOScale(2f, 0.7f).From().OnComplete(ActionManager.Instance.ActionEnd);
                    //这里return了
                    return;
                }
            }
        }
        //buff不可叠加或buff不存在，直接增添
        Debug.Log("AddBuff:\n" + _buff.GetType());
        //按优先级加入buffs
        if (_buff.Prior())
            _target.buffs.Insert(0, _buff);
        else
            _target.buffs.Add(_buff);

        _buff.Effective();
        
        //UI部分实现
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

        //动画
        _target.transform.DOShakePosition(0.6f);
        _g.transform.DOScale(2f, 0.7f).From().OnComplete(ActionManager.Instance.ActionEnd);
        

        //刷新所有敌人显示
        foreach (var _e in BattleInfo.Instance.enemies)
        {
            _e.GetComponent<EnemyBase>().ShowIntent();
        }
        //刷新所有卡牌显示
        BattleManager.Instance.AllCardRefresh();
    }

    public static void RefreshBuffUICounter(Buff _buff)
    {
        _buff.Combine_GO.GetComponentInChildren<Text>().text = _buff.num.ToString();
    }


}
