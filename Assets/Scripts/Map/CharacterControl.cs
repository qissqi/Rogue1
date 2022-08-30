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
    public int maxCost;

    /// <summary>
    /// 角色所有可用的牌库
    /// </summary>
    public SO_SO combineCards;
    //初始携带
    public GO_List basicCards;

    ///// <summary> 遗物与已装备的装备 </summary>
    //public List<Item> ActiveItem = new List<Item>();
    public GameObject BattlePlayer;
    public GameObject Attention;
    
    [Header("Control")]
    public float speed;
    private Rigidbody2D rb;
    private Animator animator;
    private float V_x, V_y;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void SetStart()
    {
        GameManager.Instance.playerInfo.SetProperty(MaxHP, MaxHP, StartGold,maxCost);
    }

    private void Update()
    {
        V_x = Input.GetAxisRaw("Horizontal");
        V_y = Input.GetAxisRaw("Vertical");
        if (GameManager.Instance.currentscene == GameManager.GameScene.Map)
        {
            animator.SetFloat("speed", Mathf.Abs(V_x) + Mathf.Abs(V_y));
            Camera.main.transform.position = transform.position + new Vector3(0, 0, -10);
            if(V_x>0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if(V_x<0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.inBattle && GameManager.Instance.currentscene == GameManager.GameScene.Map)
        {
            Move();
        }
        else if (rb.bodyType != RigidbodyType2D.Static)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(V_x*speed, V_y*speed);

    }

    public void SetAttention(bool A)
    {
        Attention.SetActive(A);
    }


}
