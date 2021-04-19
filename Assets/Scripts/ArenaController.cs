using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArenaController : MonoBehaviour
{
    [SerializeField] float spawnInterval = 2f; // interval after which a new pickup is spawned if current count < maximum count
    [SerializeField] int maximumPickups = 5; // maximum number of pickups that can exist in the scene at any time
    [SerializeField] GameObject pickupPrefab;
    [SerializeField] GameObject ui;

    [SerializeField] Button startButton;
    [SerializeField] Button exitButton;

    public int score = 0;

    bool isGamePaused = false;
    SpriteRenderer sr;
    int currentPickupCount = 0; // number of pickups that are currently in the scene

    void Start() {
        GameObject baseGo = transform.Find("Base").gameObject;
        sr = baseGo.GetComponent<SpriteRenderer>();

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
            TogglePause();
        }
    }

    void InitializeUi () {
        startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        exitButton.GetComponent<Button>().onClick.AddListener(QuitGame);
    }
    void StartGame () {
        TogglePause();
    }

    void TogglePause () {
        isGamePaused = !isGamePaused;
        Time.timeScale = isGamePaused ? 0f : 1f;
        ui.SetActive(isGamePaused ? true : false);
    }

    IEnumerator EndGame () {
        // wait for 1 seconds
        yield return new WaitForSeconds(1f);

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

    public void OnPickupCollect () {
        score++; // increase score
        currentPickupCount--; // decrease current in-game pickup count
    }

    public void OnGameEnd () {
        StartCoroutine(EndGame());
    }
}
