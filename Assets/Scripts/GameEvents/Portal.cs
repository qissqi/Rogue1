using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private bool attention;
    public int nextLevel;

    private void Update()
    {
        if (attention && (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(1)))
        {
            GameEnd();
        }
    }

    /// <summary>
    /// ½ö²âÊÔ½×¶Î
    /// </summary>
    public void GameEnd()
    {
        GameManager.Instance.LoadScene(4);
    }

    public void NextLevel()
    {
        GameManager.Instance.level++;
        GameManager.Instance.LoadNewMap();
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
