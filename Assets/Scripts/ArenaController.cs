// TODO: Arena Controller is doing too much work. Refactor. Create a new GO "Level controller".

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ArenaController : MonoBehaviour
{
    #region EDITOR VARIABLES
    [SerializeField] float spawnInterval = 2f; // interval after which a new pickup is spawned if current count < maximum count
    [SerializeField] int maximumPickups = 5; // maximum number of pickups that can exist in the scene at any time
    [SerializeField] int scoreThresholdMine = 10; // minimum score after which mines start to spawn
    [SerializeField] float mineSpawnInterval = 2f; // interval after which a new mine spawns if current mine count < maximum count
    [SerializeField] int maxMineCount = 4; // maximum number of mines that can exist in the scene at any time
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject gameUi;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI highScoreText;
    #endregion

    #region PUBLIC VARIABLES
    public SO_GameState state;
    public int score = 0;
    #endregion

    #region PRIVATE VARIABLES
    GameData gameData;
    SpriteRenderer sr;
    int currentPickupCount = 0; // number of pickups that are currently in the scene
    int currentMineCount = 0; // number of mines that are currently in the scene
    #endregion

    void Start() {
        GameObject baseGo = transform.Find("Base").gameObject;
        sr = baseGo.GetComponent<SpriteRenderer>();

        LoadGame();
        state.Initialize();
        StartCoroutine(SpawnPickups());
        StartCoroutine(SpawnMines());
    }

    void Update () {
        _CheckForPausedState();
    }

    #region PRIVATE METHODS

    IEnumerator SpawnPickups () {
        while (true) {
            yield return new WaitForSeconds(spawnInterval);
            
            SpawnPickupIfEligible();
        }
    }

    // spawn new pickup if current count is under limit
    void SpawnPickupIfEligible () {
        // exit if current count > max count
        if (currentPickupCount >= maximumPickups) {
            return;
        }

        Vector2 location = new Vector2(Random.Range(-sr.bounds.size.x / 2, sr.bounds.size.x / 2), Random.Range(-sr.bounds.size.y / 2, sr.bounds.size.y / 2));
        // Debug.Log("random location: " + location.ToString());

        // instantiate pickup at location
        Instantiate(pickupPrefab, location, Quaternion.identity);

        // increment current count
        currentPickupCount++;
    }

    IEnumerator SpawnMines () {
      while (true) {
        yield return new WaitForSeconds(mineSpawnInterval);
        SpawnMineIfEligible();
      }
    }

    // spawn new pickup if current count is under limit
    void SpawnMineIfEligible () {
      // exit if minimum score hasn't been reached
      if (score < scoreThresholdMine) {
        return;
      }

      // exit if current count > max count
      if (currentMineCount >= maxMineCount) {
        return;
      }

      Vector2 location = new Vector2(Random.Range(-sr.bounds.size.x / 2, sr.bounds.size.x / 2), Random.Range(-sr.bounds.size.y / 2, sr.bounds.size.y / 2));
        // Debug.Log("random location: " + location.ToString());

      // instantiate mine at location
      Instantiate(enemyPrefab, location, Quaternion.identity);

      // increment current count
      currentMineCount++;
    }

    void LoadGame () {
        // print(Application.persistentDataPath); // C:/Users/Debojyoti/AppData/LocalLow/DefaultCompany/Arena-2d
        GameInfo saveGame = SaveSystem.LoadGame();
        gameData = new GameData(saveGame);
    }

    void InitializeUi () {
        // initialize score UI
        scoreText.text = "Score: " + score.ToString();
        
        // initialize high score UI
        highScoreText.text = "High Score: " + gameData.highScore.ToString();
    }

    IEnumerator EndGame () {
        // wait for 1 seconds
        yield return new WaitForSeconds(1f);

        // update save game file
        SaveSystem.SaveGame(gameData, this);

        // exit to title screen
        SceneManager.LoadScene("LevelOne");
    }

    void QuitGame () {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif

        // TODO: don't quit game; send user to title screen instead
    }

    private void _CheckForPausedState () {
      Time.timeScale = state.IsGamePaused ? 0f : 1f;
      gameUi.SetActive(state.IsGamePaused ? false : true);
    }

    #endregion

    #region PUBLIC METHODS

      public void OnPickupCollect () {
        score++; // increase score
        currentPickupCount--; // decrease current in-game pickup count
        scoreText.text = "Score: " + score.ToString(); // update score UI
      }

      public void OnEnemyDestroy () {
        score++; // increase score
        currentMineCount--; // decrease current in-game enemy count
        scoreText.text = "Score: " + score.ToString(); // update score UI
      }

      public void OnGameEnd () {
        StartCoroutine(EndGame());
      }

    #endregion
}
