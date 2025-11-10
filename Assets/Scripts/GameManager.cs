using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private void Awake() {

        instance = this;
        Game.Start();

    }

    [SerializeField] private int apartmentSceneIndex = 1;

    [Header("Main Menu")]
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject mainMenuCamera;

    [Header("Days")]
    // How long should each day last in minutes.
    [SerializeField] private float dayLength = 1;
#if UNITY_EDITOR
    // If the length is in minutes (true) or seconds (false). Only in editor, purely for testing.
    [SerializeField] private bool dayLengthInMinutes = false;
#endif
    // Which day is it today.
    [SerializeField] private int day = 1;
    [SerializeField] private float difficultyMultiplier;

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Wall wall;

    [Header("Clock")]
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;

    [Header("Pause menu")]
    public bool pauseMenuOpen = false;
    [SerializeField] private GameObject pauseMenuScreen;

    [Header("Game over")]
    [SerializeField] private GameObject gameOverScreen;

    float dayEnd = 0;

    public float GetDayLength() {

#if UNITY_EDITOR       
        if (!dayLengthInMinutes)
            return dayLength;
        else
#endif
        return dayLength * 60.0f;

    }

    private void Start() {

        Game.Assert(player != null, "Player was not assigned to GameManager!");
        Game.Assert(wall != null, "Wall was not assigned to GameManager!");

        if (Game.inMainMenu)
            StartMainMenu();

        dayEnd = Time.time + GetDayLength();

    }
    private void Update() {

        float dayProgress = (GetDayLength() - (dayEnd - Time.time)) / GetDayLength();

        hourHand.transform.rotation = Quaternion.Euler(dayProgress * 2.0f * 360.0f - 90.0f, 90.0f, -90.0f);
        minuteHand.transform.rotation = Quaternion.Euler(dayProgress * 24.0f * 360.0f - 90.0f, 90.0f, -90.0f);

        if (Time.time >= dayEnd) {

            day++;
            dayEnd = Time.time + GetDayLength();

            Wall.instance.damageRate *= difficultyMultiplier;

        }

    }

    private void StartMainMenu() {

        Game.input.Disable();
        Time.timeScale = 0.5f;

        mainMenu.SetActive(true);
        mainMenuCamera.SetActive(true);

        Player.instance.alive = false;
        Player.instance.gameObject.SetActive(false);

    }

    public void GameOver() {
        
        gameOverScreen.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0.0f;

    }

    public void OpenPauseMenu() {
        
        pauseMenuScreen.SetActive(true);

        Game.input.Disable();
        Game.input.Player.Pause.Enable();

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        Time.timeScale = 0.0f;

    }
    public void ClosePauseMenu() {
        
        pauseMenuScreen.SetActive(false);

        Game.input.Player.Enable();
        Game.input.Items.Enable();

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        Time.timeScale = 1.0f;

    }

    public void StartGame() {

        Game.inMainMenu = false;

        SceneManager.LoadScene(apartmentSceneIndex);
        Time.timeScale = 1.0f;

    }
    public void Restart() {
        
        Debug.Log("Restarting game.");

        SceneManager.LoadScene(apartmentSceneIndex);
        Time.timeScale = 1.0f;

    }
    public void Quit() {
        
        Game.Quit();

    }

    public Player GetPlayer() { return GameManager.instance.player; }
    public Wall GetWall() { return GameManager.instance.wall; }

}
