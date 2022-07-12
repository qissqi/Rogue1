using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Sprites", menuName = "myItem/SpriteList")]
public class SpriteList : ScriptableObject
{
    public Sprite[] sprites;
}
