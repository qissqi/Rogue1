using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{

    public GameObject CombineUI;
    public bool attention;

    public bool createByRandomLevel;
    private int levelSet = 0;

    public Item.ItemType createItemType;


    private void Update()
    {
        if (attention && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1)) && !GameManager.Instance.inBattle)
        {
            OpenEvent();
        }
    }

    public virtual void OpenEvent()
    {
        CombineUI.SetActive(true);
        GameManager.Instance.currentscene = GameManager.GameScene.Event;
    }

    public void EventOver()
    {
        GameManager.Instance.currentscene = GameManager.GameScene.Map;
        transform.parent.GetComponent<Room>().SetMark(4, false);
        MapManager.Instance.RefreshUI();
        Destroy(gameObject);
    }

    public virtual void CreateCards(Transform parent)
    {
        var cards = GameManager.Instance.currentCharacter.combineCards.objects;
        
        if(!createByRandomLevel)
        {
            var _r = Random.Range(0, cards[0].gameObjects.Count);
            var _card = cards[levelSet].gameObjects[_r];
            Instantiate(_card, parent).GetComponent<Card>().inReward = true;
        }

    }

    public void CreateRandomItem(Transform parent)
    {
        List<List<GameObject>> items;
        switch (createItemType)
        {
            case Item.ItemType.Relic:
                items = GameManager.Instance.playerInfo.Relics_Left;
                break;
            case Item.ItemType.Equipment:
                items = GameManager.Instance.playerInfo.Equipment_Left;
                break;
            case Item.ItemType.Consumable:
                items = new List<List<GameObject>>(1);
                items[0].AddRange(GameManager.Instance.allConsumables.gameObjects);
                break;
            default:
                items = GameManager.Instance.playerInfo.Relics_Left;
                break;
        }
        if(!createByRandomLevel)
        {
            int _r = Random.Range(0, items[levelSet].Count);
            var _relic = items[levelSet][_r];
            CreateItem(_relic, parent);
        }
        else
        {
            int r1 = Random.Range(0, items.Count);
            int r2 = Random.Range(0, items[r1].Count);
            var item = GameManager.Instance.playerInfo.GetItem(createItemType, r1, r2);
            CreateItem(item,parent);
        }
    }

    private void CreateItem(GameObject _item,Transform parent)
    {
        Instantiate(_item, parent).GetComponent<Item>().Dragable = false;
    }

    public virtual void GetCard(Card card)
    {
        card.AddToPack();
        card.inReward = false;
        card.CardRespone = null;
    }
    
    public void GetItem(GameObject parent)
    {
        parent.GetComponentInChildren<Item>()?.Add();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player_Map"))
        {
            GameManager.Instance.currentCharacter.SetAttention(true);
            attention = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player_Map"))
        {
            GameManager.Instance.currentCharacter.SetAttention(false);
            attention = false;
        }
    }

}
