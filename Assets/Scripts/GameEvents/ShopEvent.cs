using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopEvent : MonoBehaviour
{
    private static ShopEvent thisLevel;
    public static ShopEvent ThisLevel { get => thisLevel; }
    public RectTransform CardPanel;
    public RectTransform ItemPanel;
    public GameObject PriceIcon;

    public void InitShop()
    {
        thisLevel = this;
        CreateCards();
        CreateItems();
    }

    bool TryBuy(int _price)
    {
        return GameManager.Instance.playerInfo.Golds >= _price;
    }

    void BuyCard(Card card)
    {
        if(TryBuy(card.price))
        {
            GameManager.Instance.playerInfo.Golds -= card.price;
            card.AddToPack();
            card.inShop = false;
            card.CardRespone = null;
            Destroy(card.transform.GetChild(card.transform.childCount-1).gameObject);
            GameManager.Instance.CloseExplainBox();
        }
        else
        {

        }
    }

    void BuyItem(Item item)
    {
        if(TryBuy(item.price))
        {
            var _block = item.transform.parent;
            item.Add();
            item.inShop = false;
            item.Dragable = true;
            item.ItemRespone = null;
            //Destroy(item.transform.parent.Find(PriceIcon.name));
            Destroy(_block.gameObject);
            GameManager.Instance.CloseExplainBox();
        }
        else
        {

        }
    }

    void CreateCards()
    {
        var cards = GameManager.Instance.currentCharacter.GetComponent<CharacterControl>().combineCards.objects;
        for (int i = 0; i < 6; i++)
        {
            var _c = Instantiate(cards[i/2].gameObjects[Random.Range(0, cards[i/2].gameObjects.Count)], CardPanel).GetComponent<Card>();
            _c.inShop = true;
            _c.price = 200 + (i / 2) * 100;
            _c.CardRespone = BuyCard;
            Instantiate(PriceIcon, _c.transform).GetComponent<Text>().text=_c.price.ToString();

        }
    }

    void CreateItems()
    {
        //遗物
        var relics = GameManager.Instance.playerInfo.Relics;
        if (relics[0].Count>0)
        {
            InitItem(relics[0], true,50);
        }
        if (relics[1].Count>0)
        {
            InitItem(relics[1], true,50);
        }

        //装备
        List<GameObject> eq = null;
        if(Random.value<0.5 && GameManager.Instance.playerInfo.Equipment[0].Count>0)
        {
            eq = GameManager.Instance.playerInfo.Equipment[0];
        }
        else if(GameManager.Instance.playerInfo.Equipment[1].Count>0)
        {
            eq = GameManager.Instance.playerInfo.Equipment[1];
        }

        if(eq!=null)
        {
            InitItem(eq, true,50);
        }

        //消耗品，不需要移除
        if(GameManager.Instance.allConsumables.gameObjects.Count>0)
        {
            InitItem(GameManager.Instance.allConsumables.gameObjects, false,50);
        }

    }

    private void InitItem(List<GameObject> list,bool remove,int pricePerLevel)
    {
        //物品生成
        GameObject block = Instantiate(InventoryManager.Instance.EmptyItemPre, ItemPanel);
        Destroy(block.transform.GetChild(0).gameObject);
        int ri = Random.Range(0, list.Count);
        var item= Instantiate(list[ri], block.transform).GetComponent<Item>();
        item.Dragable = false;
        //从所有遗物中移除，避免重复生成
        if(remove)
        {
            list.RemoveAt(ri);
        }

        //物品设置
        item.inShop = true;
        item.ItemRespone = BuyItem;

        //设定价格------目前在这里写似乎不合理
        item.price = item.Lv * pricePerLevel;
        Instantiate(PriceIcon, item.transform.parent).GetComponent<Text>().text = item.price.ToString();
    }

    public void ExitShop()
    {
        GameManager.Instance.currentscene = GameManager.GameScene.Map;
    }

}
