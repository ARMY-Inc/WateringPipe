using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject nextGameUIButton; //button prompt to move to next stage.
    public GameObject retryGameUIButton; //button prompt to retry the stage.
    public Text currentLevelText; //text that tells us which level we are on.
    public Text levelCompleteText; // notify the player if they completed the level or not.
    private static UIManager instance;

    public ADManager aDManager;
    private void Start()
    {
        instance = this;
        aDManager = GameObject.Find("AllManager").GetComponent<ADManager>();
    }
    void Update()
    {

    }

    public static IEnumerator ShowNextButtons()
    {
        if (GameManager._weWon == false && GameManager._weLost == false)
        {
            yield return new WaitForSeconds(0.0f);
            instance.currentLevelText.text = "Level " + GameManager.totalStagesPlayed;
            instance.levelCompleteText.text = "";
            instance.nextGameUIButton.SetActive(false);
            instance.retryGameUIButton.SetActive(false);
        }
        else if (GameManager._weWon == true && GameManager._weLost == false)
        {
            yield return new WaitForSeconds(0.5f);
            if (instance.nextGameUIButton.activeSelf == false)
            {
                instance.currentLevelText.text = "";
                instance.levelCompleteText.text = "Level Completed";
                instance.nextGameUIButton.SetActive(true);
                instance.retryGameUIButton.SetActive(false);
            }
            GameManager._canPlayGame = false;
        }
        else if (GameManager._weLost == true)
        {
            yield return new WaitForSeconds(0.5f);
            instance.currentLevelText.text = "";
            instance.levelCompleteText.text = "Level Failed";
            if (instance.retryGameUIButton.activeSelf == false)
            {
                instance.nextGameUIButton.SetActive(false);
                instance.retryGameUIButton.SetActive(true);
            }
            GameManager._canPlayGame = false;
        }
    }

    public void NextGame()
    {
        LevelManager.LoadNextScene();
        StartCoroutine(PlayGameYield());
        GameManager.totalStagesPlayed += 1;
        SaveSystem.SaveInformation(new PlayerData());
        aDManager.ShowMaxInterstitial(GameManager.totalStagesPlayed); //show interstitial.
    }

    public void RetryGame()
    {
        GameManager._canPlayGame = false;
        LevelManager.RetryScene();
        StartCoroutine(PlayGameYield());
    }
    private IEnumerator PlayGameYield()
    {
        yield return new WaitForSeconds(1);
        GameManager._canPlayGame = true;
    }
}
