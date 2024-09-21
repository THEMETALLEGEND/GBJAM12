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
        // Here you should ensure p_attacking's hp is properly fetched if needed
        UpdateHeartStates(6);  // Start by testing with 6 HP
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
                break;
            case 1:
                hearts_List[0].SetInteger("HeartState", 1);
                break;
            case 2:
                hearts_List[0].SetInteger("HeartState", 2);
                hearts_List[1].SetInteger("HeartState", 0);
                break;
            case 3:
                hearts_List[1].SetInteger("HeartState", 1);
                break;
            case 4:
                hearts_List[1].SetInteger("HeartState", 2);
                hearts_List[2].SetInteger("HeartState", 0);
                break;
            case 5:
                hearts_List[2].SetInteger("HeartState", 1);
                break;
            case 6:
                hearts_List[0].SetInteger("HeartState", 2);
                hearts_List[1].SetInteger("HeartState", 2);
                hearts_List[2].SetInteger("HeartState", 2);
                break;
        } // switch that transitionates the hp
        Debug.Log("Updating heart states. Current HP: " + currentHP);

        life = currentHP;
    }
}
