using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public static class SaveSystem {
  static string path = Application.persistentDataPath + "/player.dx";


  public static void SaveGame (GameData gameData, ArenaController arenaController) {
    BinaryFormatter formatter = new BinaryFormatter();
    FileStream stream = new FileStream(path, FileMode.Create);

    GameInfo gameInfo = new GameInfo(gameData, arenaController);

    formatter.Serialize(stream, gameInfo);
    stream.Close();
  }

  public static GameInfo LoadGame () {
    if (File.Exists(path)) {
      BinaryFormatter formatter = new BinaryFormatter();
      FileStream stream = new FileStream(path, FileMode.Open);

      GameInfo gameInfo = formatter.Deserialize(stream) as GameInfo;

      stream.Close();
      return gameInfo;
    } else {
      Debug.Log("Save file not found");
      return null;
    }
  }
}