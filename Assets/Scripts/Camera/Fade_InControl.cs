using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GBTemplate;

public class Fade_InControl : MonoBehaviour
{
    public GBConsoleController disp;
    public Canvas canvasText;
    void Awake()
    {
        canvasText.enabled = false;
        StartCoroutine(FadeFromB());
    }
    private IEnumerator FadeFromB()
    {
        yield return disp.Display.StartCoroutine(disp.Display.FadeFromBlack(2));
        canvasText.enabled = true;
    } 
    public IEnumerator ChangeScene()
    {
        yield return disp.Display.StartCoroutine(disp.Display.FadeToBlack(2));   
    }
}
