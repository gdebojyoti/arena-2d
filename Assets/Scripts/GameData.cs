// TODO: use this to store important data related to the game; offload some of the work from Level Controller

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData {
    public string playerName = "Untitled";
    public int highScore = 0; // highest score from all of previous matches
    public int lastScore = 0; // previous score
    public int currentScore = 0; // score in current game
    
    public GameData (GameInfo saveGame) {
        if (saveGame == null) {
            Debug.Log("No save game data found..");
            return;
        }

        this.highScore = saveGame.highScore;
        this.lastScore = saveGame.lastScore;

        Debug.Log("Game loaded. HS: " + highScore + " LS: " + lastScore);
    }
}
