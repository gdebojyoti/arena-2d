// TODO: Arena Controller is doing too much work. Refactor. Create a new GO "Level controller".

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ArenaController : MonoBehaviour
{
    [SerializeField] float spawnInterval = 2f; // interval after which a new pickup is spawned if current count < maximum count
    [SerializeField] int maximumPickups = 5; // maximum number of pickups that can exist in the scene at any time
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] GameObject menuUi;
    [SerializeField] GameObject gameUi;

    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;
    [SerializeField] TextMeshProUGUI scoreText;

    public int score = 0;

    GameData gameData;
    bool hasStarted = false; // whether game has started
    bool isGamePaused = false; // whether game is paused
    SpriteRenderer sr;
    int currentPickupCount = 0; // number of pickups that are currently in the scene

    void Start() {
        GameObject baseGo = transform.Find("Base").gameObject;
        sr = baseGo.GetComponent<SpriteRenderer>();

        LoadGame();
        InitializeUi();
        TogglePause();
        StartCoroutine(SpawnPickups());
    }

    void Update () {
        CheckForInputs();
    }

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

    void CheckForInputs () {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (this.hasStarted) {
                TogglePause();
            }
        }
    }

    void LoadGame () {
        // print(Application.persistentDataPath); // C:/Users/Debojyoti/AppData/LocalLow/DefaultCompany/Arena-2d
        GameInfo saveGame = SaveSystem.LoadGame();
        gameData = new GameData(saveGame);
    }

    void InitializeUi () {
        startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        exitButton.GetComponent<Button>().onClick.AddListener(QuitGame);

        scoreText.text = "Score: " + score.ToString();
    }
    void StartGame () {
        TogglePause();
        
        // update flag
        this.hasStarted = true;
        
        // change start button's text to "Continue"
        GameObject startGo = startButton.gameObject;
        GameObject startText = startGo.transform.Find("text").gameObject;
        startText.GetComponent<TextMeshProUGUI>().text = "Continue";
    }

    void TogglePause () {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
        menuUi.SetActive(isGamePaused ? true : false);
        gameUi.SetActive(isGamePaused ? false : true);
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

    // public methods

    #region public methods

    public void OnPickupCollect () {
        score++; // increase score
        currentPickupCount--; // decrease current in-game pickup count
        scoreText.text = "Score: " + score.ToString(); // update score UI
    }

    public void OnGameEnd () {
        StartCoroutine(EndGame());
    }

    #endregion
}
