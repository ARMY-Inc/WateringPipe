[System.Serializable]
public class PlayerData
{
    public int lastStage = 1;// the last stage we played/unlocked
    public int lastStageTotal;

    public PlayerData() //save what level we were on.
    {
        lastStage = GameManager.currentStage;
        lastStageTotal = GameManager.totalStagesPlayed;
    }
}

