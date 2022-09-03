using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprites", menuName = "myItem/SpriteList")]
public class SpriteList : ScriptableObject
{
    public Sprite[] sprites;

    public Dictionary<string, Sprite> Sprite_Dic = new Dictionary<string, Sprite>();

    public bool Fresh;
    private void OnValidate()
    {
        FreshDic();
    }

    public void FreshDic()
    {
        Sprite_Dic.Clear();
        foreach (var sp in sprites)
        {
            Sprite_Dic.Add(sp.name, sp);
        }
        Debug.Log(Sprite_Dic.Keys.Count);
    }
}
