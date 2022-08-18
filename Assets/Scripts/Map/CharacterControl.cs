using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 角色操控，角色信息与道具，与战斗的player脚本关联
/// </summary>
public class CharacterControl : MonoBehaviour
{
    public int MaxHP;
    public int StartGold;

    /// <summary>
    /// 角色所有可用的牌库
    /// </summary>
    public SO_SO combineCards;

    /// <summary> 遗物与已装备的装备 </summary>
    public List<Item> ActiveItem = new List<Item>();
    public GameObject BattlePlayer;
    public GameObject Attention;
    
    public float speed;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        GameManager.Instance.playerInfo.SetProperty(MaxHP, MaxHP, StartGold);
    }

    private void Update()
    {
        if (GameManager.Instance.currentscene == GameManager.GameScene.Map)
        {
            Camera.main.transform.position = transform.position + new Vector3(0, 0, -10);
        }
    }

    private void FixedUpdate()
    {
        if(!GameManager.Instance.inBattle&& GameManager.Instance.currentscene == GameManager.GameScene.Map)
        {
            Move();
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Move()
    {
        float Ix = Input.GetAxisRaw("Horizontal");
        float Iy = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(Ix*speed, Iy*speed);

    }

    public void SetAttention(bool A)
    {
        Attention.SetActive(A);
    }


}
