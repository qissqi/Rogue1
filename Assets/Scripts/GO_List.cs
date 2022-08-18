using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "myItem/GO_List",fileName = "GO_List",order =3)]
public class GO_List : ScriptableObject
{
    public List<GameObject> gameObjects = new List<GameObject>();

}
