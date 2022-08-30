using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Enemy_OnMap : MonoBehaviour//,IPointerEnterHandler,IPointerExitHandler,IPointerClickHandler
{
    public GameObject[] combineEnemyPre;
    private bool attention;
    public int EnemyLevel;

    private void Update()
    {
        if(attention&&(Input.GetKeyDown(KeyCode.E)||Input.GetMouseButtonDown(1))&&!GameManager.Instance.inBattle)
        {
            BattleManager.Instance.BattleStart(this);
        }
    }

    public void BattleEnd(bool win)
    {
        if(win)
        {
            MapManager.Instance.currentRoom.SetDoor(true);
            MapManager.Instance.currentRoom.EnterLock = false;
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player_Map"))
        {
            GameManager.Instance.currentCharacter.SetAttention(true);
            attention = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Player_Map"))
        {
            GameManager.Instance.currentCharacter.SetAttention(false);
            attention = false;
        }
    }

}
