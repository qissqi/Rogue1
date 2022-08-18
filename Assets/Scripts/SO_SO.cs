using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SO",menuName = "myItem/SO_SO",order = 4)]
public class SO_SO : ScriptableObject
{
    public List<GO_List> objects;
}
