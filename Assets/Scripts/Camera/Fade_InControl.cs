using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GBTemplate;
using UnityEngine.SceneManagement;

public class Fade_InControl : MonoBehaviour
{
    public Canvas canvasText;
    private GBConsoleController disp; 

    void Awake()
    {
        canvasText.enabled = false;


        disp = FindObjectOfType<GBConsoleController>();

        if (disp != null)
        {
            StartCoroutine(FadeFromB());
        }
        else
        {
            Debug.LogError("GBConsoleController not found in the scene!");
        }
    }

    private IEnumerator FadeFromB()
    {
        yield return disp.Display.StartCoroutine(disp.Display.FadeFromBlack(2));
        canvasText.enabled = true;
    }

    public IEnumerator ChangeScene()
    {
        yield return disp.Display.StartCoroutine(disp.Display.FadeToBlack(2));
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        switch(currentSceneIndex){
            case 1:
                SceneManager.LoadScene(2);
                break;
            case 2:
                SceneManager.LoadScene(3);
                break;
            case 3:
                SceneManager.LoadScene(5);
                break;
        }
        
    }

    public void ReloadSceneMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void EndGame()
    {
        StartCoroutine(EndingGame());   
    }
    private IEnumerator EndingGame()
    {
        yield return disp.Display.StartCoroutine(disp.Display.FadeToBlack(2));
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
