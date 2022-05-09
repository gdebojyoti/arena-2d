using UnityEngine;
using UnityEngine.UIElements;

public class UiService : MonoBehaviour {

  #region EDITOR VARIABLES
    [SerializeField] GameObject mainMenuGo;
    public SO_GameState state;
  #endregion

  #region PRIVATE VARIABLES
    private VisualElement body;
    private Button startBtn;
  #endregion

  #region MONOBEHAVIOR METHODS

    void Start() {
      _InitializeElements();
    }

    void Update () {
      _UpdateMainMenu();
    }

  #endregion

  #region PRIVATE METHODS

    private void _InitializeElements () {
      VisualElement root = mainMenuGo.GetComponent<UIDocument>().rootVisualElement;
      body = root.Q<VisualElement>("body");
      startBtn = root.Q<Button>("start-button");
      // startBtn.clicked += _StartOrResumeGame;
      startBtn.RegisterCallback<ClickEvent>(_StartOrResumeGame);
      var exitBtn = root.Q<Button>("exit-button");
      exitBtn.clicked += _QuitGame;
    }

    private void _StartOrResumeGame (ClickEvent evt) {
        state.TogglePauseGame();
        _UpdateMainMenu();

        // exit if game has already started
        if (state.HasStarted) {
          return;
        }
        
        // update flag
        state.StartGame();

        // change start button's text to "Continue"
        startBtn.text = "CONTINUE";
    }

    private void _QuitGame () {
      Debug.Log("quitting...");
      #if UNITY_EDITOR
          UnityEditor.EditorApplication.isPlaying = false;
      #else
          Application.Quit();
      #endif

      // TODO: don't quit game; send user to title screen instead
    }

    // show / hide main menu
    private void _UpdateMainMenu () {
      body.style.display = state.IsGamePaused
        ? DisplayStyle.Flex
        : DisplayStyle.None;
    }

  #endregion
}