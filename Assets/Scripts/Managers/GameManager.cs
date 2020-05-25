using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARMY.MasterData;
public class GameManager : MonoBehaviour
{
    public static int currentStage = 1; //the current stage/scene we are playing. (Level 1. Level 15, etc.)
    public static int totalStagesPlayed = 1; //are we on stage 10, 11,12, etc (despite which scene we loaded)
    public static bool _canPlayGame; //can we play the game/move books? (disabled for tutorials and startup menu)

    public static bool _weWon;
    public static bool _weLost;
    public MasterDataSO ourManager;

    [Header("other managers")]
    public static GameManager instance;
    public LevelManager levelManager;
    public static ADManager aDManager;


    private void Awake()
    {
        instance = this;
        // SaveSystem.DeleteInformation();
        SaveSystem.LoadInformation();
        LevelManager.FirstScene();
    }
    // Start is called before the first frame update
    private void Start()
    {
        _canPlayGame = true;
        aDManager = GameObject.Find("AllManager").GetComponent<ADManager>();
    }
    private void Update()
    {

    }

    static public IEnumerator WeWonFunction()
    {
        _canPlayGame = false;
        yield return new WaitForSeconds(3.0f);
        _weWon = true;
        instance.StartCoroutine(UIManager.ShowNextButtons());
    }
    static public void WeLostFunction()
    {
        if (_weWon == false)
        {
            _canPlayGame = false;
            _weLost = true;
            instance.StartCoroutine(UIManager.ShowNextButtons());
        }
    }
}
