using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    public static void Save(PlayerController player, GameManager gameManager, int saveSlot)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/save" + saveSlot.ToString();
        FileStream stream = new FileStream(path, FileMode.Create);
        SaveData data = new SaveData(player, gameManager);
        formatter.Serialize(stream, data);
        stream.Close();
    }    

    public static SaveData Load(int saveSlot)
    {
        string path = Application.persistentDataPath + "/save" + saveSlot.ToString();
        // Debug.Log(path);
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();
            return data;
        }
        else
        {
            return null;
        }
    }
}
