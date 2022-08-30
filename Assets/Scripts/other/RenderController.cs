using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderController : MonoBehaviour
{
    [SerializeField]private CharacterControl player;
    [SerializeField]private Renderer spr;
    public float offset;
    [SerializeField]bool up = true;
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
            spr.sortingOrder += 2;
        }
        else if(player.transform.position.y+offset<transform.position.y && up)
        {
            up = false;
            spr.sortingOrder -= 2;

        }
    }
}
