using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NOTE:
// all variables in this class must be serializable
// i.e., each variable must belong to one of the 4 basic data types -
// string, bool, float, int
// arrays are supported
// the helper functions will be responsible for converting to Unity specific data types (like Vector 3)
// to something that is serializable (like an array of floats)

[System.Serializable]
public class GameInfo {
  public string playerName = "Untitled";
  public int highScore = 0; // highest score from all of previous matches
  public int lastScore = 0; // previous score
  public int currentScore = 999; // score in current game

  public GameInfo (GameData gameData, ArenaController arenaController) {
    this.highScore = arenaController.score > gameData.highScore ? arenaController.score : gameData.highScore;
    this.lastScore = arenaController.score;
  }
}