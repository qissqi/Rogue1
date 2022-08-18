using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��ɫ�ٿأ���ɫ��Ϣ����ߣ���ս����player�ű�����
/// </summary>
public class CharacterControl : MonoBehaviour
{
    public int MaxHP;
    public int StartGold;

    /// <summary>
    /// ��ɫ���п��õ��ƿ�
    /// </summary>
    public SO_SO combineCards;

    /// <summary> ��������װ����װ�� </summary>
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
