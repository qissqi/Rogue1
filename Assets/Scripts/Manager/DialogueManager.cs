using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueManager : Singleton<DialogueManager>
{
    public Text name_Text,content_text;

    [Header("Debug")]
    public List<string> Rows = new List<string>();
    public List<string> Marks = new List<string>();
    public Dictionary<string, List<int>> textDic = new Dictionary<string, List<int>>();
    public bool useParts;
    public Action onEnd;

    public int page, maxPage;
    public bool canContinue;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        gameObject.SetActive(false);
    }

    public void LoadText(TextAsset textAsset)
    {
        Rows.Clear();
        textDic.Clear();
        Marks.Clear();
        var Text = textAsset.text;
        Rows.AddRange(Text.Split('\n'));
        switch(Rows[0].Split(',')[0])
        {
            case "P":
                useParts = true;
                break;

            default:
                useParts = false;
                break;
        }

        if(useParts)
        {
            CutPart();
        }
    }

    public void CutPart()
    {
        string partMark = null;
        List<int> group = new List<int>();
        for (int i = 1; i < Rows.Count; i++)
        {
            string[] cells = Rows[i].Split(',');
            //标记不同
            if (partMark != cells[0])
            {
                //存下当前组
                if(partMark!=null)
                {
                    textDic.Add(partMark, group);
                    Marks.Add(partMark);
                    group = new List<int>();
                }
                partMark = cells[0];
                group.Add(i);
                
            }
            //标记相同
            else
            {
                group.Add(i);
            }
        }
    }

    public void StartDialogue(string mark = null,Action _onEnd = null)
    {
        canContinue = true;
        onEnd = _onEnd;
        gameObject.SetActive(true);
        //普通
        if(!useParts)
        {
            string _alltext = "";
            for (int i = 1; i < Rows.Count-1; i++)
            {
                var _name = Rows[i].Split(',')[1];
                var _text = Rows[i].Split(',')[2];
                _alltext += _name+"："+_text+"\n";
            }
            print(_alltext);

            maxPage = Rows.Count;
            page = 1;
            var _d = Rows[1].Split(',');
            ShowDialogue(_d[1], _d[2]);

        }
        //随机取组
        else if (mark == null)
        {
            int _r = UnityEngine.Random.Range(0, Marks.Count);
            page = textDic[Marks[_r]][0];
            maxPage = page + textDic[Marks[_r]].Count+1;
            var _d = Rows[page].Split(',');
            ShowDialogue(_d[1], _d[2]);
        }
        //指定组
        else
        {
            page = textDic[mark][0];
            maxPage = page + textDic[mark].Count;
            var _d = Rows[page].Split(',');
            ShowDialogue(_d[1], _d[2]);
        }
    }

    public void GoNext()
    {
        if(!canContinue)
        {
            var _d = Rows[page].Split(',');
            ShowDialogue(_d[1], _d[2]);
            return;
        }

        page++;
        if(page<maxPage-1)
        {
            var _d = Rows[page].Split(',');
            ShowDialogue(_d[1], _d[2]);
        }
        else
        {
            EndDialogue();
        }
    }
    public void ShowDialogue(string _name,string _text)
    {
        name_Text.text = _name;
        if(canContinue)
        {
            canContinue = false;
            StartCoroutine(ShowOneByOne(_text,0.05f));
        }
        else
        {
            StopAllCoroutines();
            content_text.text = _text;
            canContinue = true;
        }
    }

    private IEnumerator ShowOneByOne(string _text,float time)
    {
        string t = "";
        for (int i = 0; i < _text.Length; i++)
        {
            t += _text[i];
            content_text.text = t;
            yield return new WaitForSeconds(time);
        }
        canContinue = true;
    }

    public void EndDialogue()
    {
        gameObject.SetActive(false);
        onEnd();
    }



    /// <summary>
    /// 只返回一句
    /// </summary>
    public string GetTextPart(string mark = null,int index=0)
    {
        int row = textDic[mark][index];
        var cells = Rows[row].Split(',');
        string text = cells[2];
        return text;
    }


}
