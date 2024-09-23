using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GBTemplate;

public class changeMusic_1 : MonoBehaviour
{
    private GBSoundController gbControll;
    public AudioClip theme1;
    public AudioClip theme2;
    public AudioClip theme3;

    void Start()
    {
        gbControll = FindObjectOfType<GBSoundController>();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch (currentSceneIndex) {
            case 1:
                gbControll.PlayMusic(theme1);
                break;
            case 2:
                gbControll.PlayMusic(theme2);
                break;
            case 3:
                gbControll.PlayMusic(theme3);
                break;
            default:
                break;
        }

    }
}
