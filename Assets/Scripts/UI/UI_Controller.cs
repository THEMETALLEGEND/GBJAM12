using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    public GameObject Hearts; 
    public GameObject Coins;
    public GameObject p_attacking;
    List<GameObject> hearts_List = new List<GameObject>();
    public TextMeshProUGUI coins_counter;



    public float heartSpacing = 50f; 
    public Vector2 startingPosition = new Vector2(-200f, 100f); 

    void Start()
    {
        int totalHearts = p_attacking.GetComponent<PlayerAttacking>().hp;
        UpdateHeartUI(totalHearts);
    }
    public void UpdateCoins(int amount)
    {
        string textToUi;
        if (amount < 10)
        {
            textToUi = "0" + amount;
        }
        else
        {
            textToUi = amount.ToString();
        }
        coins_counter.text = textToUi;
    }
    public void UpdateLife(bool isSubtracting, int value)
    {
        if (isSubtracting)
        {
            for (int i = 0; i < value; i++)
            {
                if (hearts_List.Count > 0)
                {
                    Destroy(hearts_List[hearts_List.Count - 1]);
                    hearts_List.RemoveAt(hearts_List.Count - 1);
                }
            }
        }
        else
        {
            for (int i = 0; i < value; i++)
            {
                AddHeart();
            }
        }

        AlignHearts(); 
    }

    void UpdateHeartUI(int totalHearts)
    {
        for (int i = 0; i < totalHearts; i++)
        {
            AddHeart();
        }
        AlignHearts(); 
    }

    void AddHeart()
    {
        GameObject heart = Instantiate(Hearts, Vector2.zero, Quaternion.identity); 
        heart.transform.SetParent(this.transform, false); 
        hearts_List.Add(heart); 
    }

    void AlignHearts()
    {
        for (int i = 0; i < hearts_List.Count; i++)
        {
            RectTransform heartRect = hearts_List[i].GetComponent<RectTransform>();
            heartRect.anchoredPosition = new Vector2(startingPosition.x + i * heartSpacing*10, startingPosition.y); 
        }
    }
}
