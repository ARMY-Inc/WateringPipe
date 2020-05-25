using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary; //encodes the data, so it cannot be edited.
using UnityEngine.SceneManagement;

public static class SaveSystem
{
    #region GameInformation
    public static void SaveInformation(PlayerData data)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/WateringPipe.game";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadInformation()
    {

        string path = Application.persistentDataPath + "/WateringPipe.game";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            GameManager.currentStage = data.lastStage;
            GameManager.totalStagesPlayed = data.lastStageTotal;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("save file not found in " + path);
            return null;
        }
    }

    public static void DeleteInformation()
    {
        string path = Application.persistentDataPath + "/WateringPipe.game";

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.LogError("file deleted");
        }
    }
    #endregion
}