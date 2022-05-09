// details of a block that can occupy a cell

using UnityEngine;

[CreateAssetMenu(fileName = "Game state", menuName = "Arena 2D/Game State")]
public class SO_GameState : ScriptableObject {
  // public new string name;

  [SerializeField] private bool hasGameStarted = false; // whether game has started
  public bool HasGameStarted { get { return hasGameStarted; } }
  [SerializeField] private bool isGamePaused = true; // whether game is paused
  public bool IsGamePaused { get { return isGamePaused; } }

  public void Initialize () {
    hasGameStarted = false;
    isGamePaused = true;
  }

  public void StartGame () {
    hasGameStarted = true;
  }

  public void TogglePauseGame () {
    isGamePaused = !isGamePaused;
  }
}