using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopEvent : MonoBehaviour
{
    public GameObject CombineUI;
    public bool attention;
    private static ShopEvent thisLevel;

    public static ShopEvent ThisLevel { get => thisLevel; }
    public GameObject say;
    public GameObject eee;
    public RectTransform CardPanel;
    public RectTransform ItemPanel;
    public GameObject PriceIcon;
    public TextAsset text_first;
    public TextAsset text_say;
    public int buyNum = 0;
    public int failNum = 0;
    public TMPro.TMP_Text goldsText;


    private void Update()
    {
        if (attention && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1)) && !GameManager.Instance.inBattle)
        {
            OpenEvent();
        }
    }

    public void FreshGolds()
    {
        goldsText.text = GameManager.Instance.playerInfo.Golds.ToString();
    }

    public void OpenEvent()
    {
        CombineUI.SetActive(true);
        goldsText.text = GameManager.Instance.playerInfo.Golds.ToString();
        GameManager.Instance.ChangeScene(GameManager.GameScene.Event);
        failNum = 0;
        if(!GameManager.Instance.firstShop)
        {
            DialogueManager.Instance.LoadText(text_first);
            DialogueManager.Instance.StartDialogue(null,LoadSay);
            GameManager.Instance.firstShop = true;
        }
        else
        {
            LoadSay();
        }
    }

    public void LoadSay()
    {
        DialogueManager.Instance.LoadText(text_say);
    }

    public void InitShop()
    {
        thisLevel = this;
        CreateCards();
        CreateItems();
    }

    public void Buy_Conversation(bool success)
    {
        StopAllCoroutines();
        eee.SetActive(false);
        string text;
        if(success)
        {
            failNum = 0;
            buyNum++;
            if(buyNum>=5)
            {
                text = DialogueManager.Instance.GetTextPart("ok3");
            }
            else
            {
                text = DialogueManager.Instance.GetTextPart(Random.value < 0.5 ? "ok1" : "ok2");
            }
        }
        else
        {
            failNum++;
            if(failNum<5)
            {
                text = DialogueManager.Instance.GetTextPart(Random.value < 0.5 ? "no1" : "no2");
            }
            else if(failNum==5)
            {
                text = DialogueManager.Instance.GetTextPart("no3");
            }
            else if(failNum<10)
            {
                float r = Random.value;
                text = DialogueManager.Instance.GetTextPart(r < 0.33 ? "no1" : r < 0.66 ? "no2" : "no3");
            }
            else
            {
                eee.SetActive(true);
                text = DialogueManager.Instance.GetTextPart("no4");
            }
        }

        StartCoroutine(conversation(text, 3f));
    }

    public IEnumerator conversation(string text,float time)
    {
        string _text = "";
        float _time = 0;
        say.SetActive(true);
        Text textBox = say.transform.GetChild(0).GetComponent<Text>();

        for (int i = 0; i < text.Length; i++)
        {
            _text += text[i];
            textBox.text = _text;
            yield return new WaitForSeconds(0.05f);
            _time += 0.05f;
        }

        while (_time<time)
        {
            yield return null;
            _time += Time.deltaTime;
        }

        say.SetActive(false);
        eee.SetActive(false);
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
            goldsText.text = GameManager.Instance.playerInfo.Golds.ToString();
            card.AddToPack();
            card.inSelect = false;
            card.CardRespone = null;
            Destroy(card.transform.GetChild(card.transform.childCount-1).gameObject);
            GameManager.Instance.CloseExplainBox();

            Buy_Conversation(true);
        }
        else
        {
            Buy_Conversation(false);

        }
    }

    void BuyItem(Item item)
    {
        if(TryBuy(item.price))
        {
            GameManager.Instance.playerInfo.Golds -= item.price;
            goldsText.text = GameManager.Instance.playerInfo.Golds.ToString();
            var _block = item.transform.parent;
            item.Add();
            item.useCallback = false;
            item.Dragable = true;
            item.ItemRespone = null;
            //Destroy(item.transform.parent.Find(PriceIcon.name));
            Destroy(_block.gameObject);
            GameManager.Instance.CloseExplainBox();

            Buy_Conversation(true);
        }
        else
        {
            Buy_Conversation(false);
        }
    }

    public void CreateCards()
    {
        var cards = GameManager.Instance.currentCharacter.combineCards.objects;
        for (int i = 0; i < 6; i++)
        {
            var _c = Instantiate(cards[i/2].gameObjects[Random.Range(0, cards[i/2].gameObjects.Count)], CardPanel).GetComponent<Card>();
            _c.inSelect = true;
            _c.price = 200 + (i / 2) * 100;
            _c.CardRespone = BuyCard;
            Instantiate(PriceIcon, _c.transform).GetComponent<Text>().text=_c.price.ToString();

        }
    }

    void CreateItems()
    {
        //遗物
        var relics = GameManager.Instance.playerInfo.Relics_Left;
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
        if(Random.value<0.5 && GameManager.Instance.playerInfo.Equipment_Left[0].Count>0)
        {
            eq = GameManager.Instance.playerInfo.Equipment_Left[0];
        }
        else if(GameManager.Instance.playerInfo.Equipment_Left[1].Count>0)
        {
            eq = GameManager.Instance.playerInfo.Equipment_Left[1];
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
            //list.RemoveAt(ri);
            GameManager.Instance.playerInfo.NewGetItem(item);
        }

        //物品设置
        item.useCallback = true;
        item.ItemRespone = BuyItem;

        //设定价格------目前在这里写似乎不合理
        item.price = item.Lv * pricePerLevel;
        Instantiate(PriceIcon, item.transform.parent).GetComponent<Text>().text = item.price.ToString();
    }

    public void ExitShop()
    {
        StopAllCoroutines();
        say.SetActive(false);
        eee.SetActive(false);
        GameManager.Instance.ChangeScene(GameManager.GameScene.Map);
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
