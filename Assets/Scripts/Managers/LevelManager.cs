using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    //this script controls loading/unloading scenes. it also controls the levels/scenes we want to load.
    private int whichScene;
    private static LevelManager instance;
    [HideInInspector] public Scene nextScene; //this checks if the scene/level was loaded, (so we can assign variables to GameManager)

    private void Start() 
    {
        instance = this;
        instance.StartCoroutine(UIManager.ShowNextButtons());
    }
    public static void FirstScene()
    {

        SceneManager.LoadSceneAsync("Level " + GameManager.currentStage, LoadSceneMode.Additive); //load our next level. 
    }

    public static void LoadNextScene()
    {
        SceneManager.UnloadSceneAsync("Level " + GameManager.currentStage);
        GameManager.currentStage++;
        if (GameManager.currentStage > (SceneManager.sceneCountInBuildSettings - 1)) //if we have reached the last stage, then start over at Stage 1.
        {
            GameManager.currentStage = 1;
        }
        SceneManager.LoadSceneAsync("Level " + GameManager.currentStage, LoadSceneMode.Additive); //load our next level.
        GameManager._weWon = false;
        GameManager._weLost = false;
        instance.StartCoroutine(UIManager.ShowNextButtons());
    }

    public static void RetryScene()
    {
        SceneManager.UnloadSceneAsync("Level " + GameManager.currentStage);
        SceneManager.LoadSceneAsync("Level " + GameManager.currentStage, LoadSceneMode.Additive); //load our next level.
        GameManager._weWon = false;
        GameManager._weLost = false;
        instance.StartCoroutine(UIManager.ShowNextButtons());
    }

    //Because we cant assign to variables at once with the Button UI, we need to create two functions to assign it.
    #region Choose Which Stage and Level to Load
    public void AssignChosenStage(int ChosenScene) //what Scene are we playing?
    {
        whichScene = ChosenScene;
    }
    //this function is called when we select an unlocked scene and level from the LevelSelector UI
    public void LoadChosenLevel()
    {
        GameManager.currentStage = whichScene - 1;
        LoadNextScene();
    }
    #endregion
}
