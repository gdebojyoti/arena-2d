using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem {
  static string path = Application.persistentDataPath + "/player.dx";


  public static void SavePlayer (int score) {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream stream = new FileStream(path, FileMode.Create);

    PlayerData data = new PlayerData(score);

    formatter.Serialize(stream, data);
    stream.Close();
  }

  public static PlayerData LoadPlayer () {
    if (File.Exists(path)) {
      BinaryFormatter formatter = new BinaryFormatter();
      FileStream stream = new FileStream(path, FileMode.Open);

      PlayerData data = formatter.Deserialize(stream) as PlayerData;

      stream.Close();
      return data;
    } else {
      Debug.Log("Save file not found");
      return null;
    }
  }
}