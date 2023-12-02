using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlScene : MonoBehaviour
{
    public void ChangeSceneWithIndexBuild(int buildIndex)
    {
        if (buildIndex == 0) Screen.orientation = ScreenOrientation.Portrait;
        if (buildIndex == 1) Screen.orientation = ScreenOrientation.LandscapeLeft;
        UnityEngine.SceneManagement.SceneManager.LoadScene(buildIndex);
    }
   
    public void QuitApp()
    {
        Application.Quit();
    }
} 
