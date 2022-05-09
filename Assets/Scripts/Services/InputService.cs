using UnityEngine;
using UnityEngine.UIElements;

public class InputService : MonoBehaviour {

  #region EDITOR VARIABLES
    public SO_GameState state;
  #endregion

  #region MONOBEHAVIOR METHODS

    void Update () {
      _CheckForInputs();
    }

  #endregion

  #region PRIVATE METHODS

    private void _CheckForInputs () {
      if (Input.GetKeyDown(KeyCode.Escape)) {
          if (state.HasGameStarted) {
              state.TogglePauseGame();
          }
      }
    }

  #endregion
}