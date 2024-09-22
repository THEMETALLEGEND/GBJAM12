using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GBTemplate;

public class Fade_InControl : MonoBehaviour
{
    public GBDisplayController disp;
    // Start is called before the first frame update
    void Start()
    {
        disp.FadeFromBlack(2);
    }

}
