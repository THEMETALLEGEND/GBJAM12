using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UI_Controller : MonoBehaviour
{
    public GameObject Coins;
    public GameObject p_attacking;
    public List<Animator> hearts_List = new List<Animator>();
    public TextMeshProUGUI coins_counter;

    [HideInInspector] public int life;

    void Start()
    {
        hearts_List.Add(null);
        hearts_List.Add(null);
        hearts_List.Add(null);

        UpdateHeartStates(6);
    }

    void Update()
    {
        hearts_List[0] = transform.Find("Heart").GetComponent<Animator>();
        hearts_List[1] = transform.Find("Heart (1)").GetComponent<Animator>();
        hearts_List[2] = transform.Find("Heart (2)").GetComponent<Animator>();
    }

    public void UpdateCoins(int amount)
    {
        string textToUi = amount < 10 ? "0" + amount : amount.ToString();
        coins_counter.text = textToUi;
    }

    public void UpdateHeartStates(int currentHP)
    {
        switch (currentHP)
        {
            case 0:
                hearts_List[0].SetInteger("HeartState", 0);
                hearts_List[1].SetInteger("HeartState", 0);
                hearts_List[2].SetInteger("HeartState", 0);
                break;
            case 1:
                hearts_List[0].SetInteger("HeartState", 1);
                hearts_List[1].SetInteger("HeartState", 0);
                hearts_List[2].SetInteger("HeartState", 0);
                break;
            case 2:
                hearts_List[0].SetInteger("HeartState", 2);
                hearts_List[1].SetInteger("HeartState", 0);
                hearts_List[2].SetInteger("HeartState", 0);
                break;
            case 3:
                hearts_List[0].SetInteger("HeartState", 2);
                hearts_List[1].SetInteger("HeartState", 1);
                hearts_List[2].SetInteger("HeartState", 0);
                break;
            case 4:
                hearts_List[0].SetInteger("HeartState", 2);
                hearts_List[1].SetInteger("HeartState", 2);
                hearts_List[2].SetInteger("HeartState", 0);
                break;
            case 5:
                hearts_List[0].SetInteger("HeartState", 2);
                hearts_List[1].SetInteger("HeartState", 2);
                hearts_List[2].SetInteger("HeartState", 1);
                break;
            case 6:
                hearts_List[0].SetInteger("HeartState", 2);
                hearts_List[1].SetInteger("HeartState", 2);
                hearts_List[2].SetInteger("HeartState", 2);
                break;
        }
        life = currentHP;
    }
}
