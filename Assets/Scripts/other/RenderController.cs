using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderController : MonoBehaviour
{
    [SerializeField]private CharacterControl player;
    [SerializeField]private Renderer spr;
    public float offset;
    [SerializeField]bool up = true;
    public bool revers;
    private void Start()
    {
        player = GameManager.Instance.currentCharacter;
        spr = GetComponent<Renderer>();
    }

    private void Update()
    {
        if(player.transform.position.y+offset>transform.position.y && !up)
        {
            up = true;
            if(!revers)
                spr.sortingOrder += 2;
            else
                spr.sortingOrder -= 2;
        }
        else if(player.transform.position.y+offset<transform.position.y && up)
        {
            up = false;
            if(!revers)
                spr.sortingOrder -= 2;
            else
                spr.sortingOrder += 2;

        }
    }
}
