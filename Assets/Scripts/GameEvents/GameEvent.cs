using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{

    public GameObject CombineUI;
    public bool attention;

    private void Update()
    {
        if (attention && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1)) && !GameManager.Instance.inBattle)
        {
            CombineUI.SetActive(true);
            GameManager.Instance.currentscene = GameManager.GameScene.Event;
        }
    }

    public void GetItem(GameObject parent)
    {
        parent.GetComponentInChildren<Item>()?.Add();
    }

    public void EventOver()
    {
        GameManager.Instance.currentscene = GameManager.GameScene.Map;
        transform.parent.GetComponent<Room>().SetMark(4, false);
        Destroy(gameObject);
    }


    public void MakeRandomRelic(Transform aimParent)
    {

    }

    public void MakeRandomConsumables(Transform aimParent)
    {

    }
    public void MakeRandomEquipment(Transform aimParent)
    {

    }




    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player_Map"))
        {
            MapManager.Instance.currentPlayerControl.SetAttention(true);
            attention = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player_Map"))
        {
            MapManager.Instance.currentPlayerControl.SetAttention(false);
            attention = false;
        }
    }

}
