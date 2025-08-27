using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private void Awake() {

        instance = this;
        Game.Start();

    }

    [Header("Days")]
    // How long should each day last in minutes.
    [SerializeField] private float dayLength = 1;
#if UNITY_EDITOR
    // If the length is in minutes (true) or seconds (false). Only in editor, purely for testing.
    [SerializeField] private bool dayLengthInMinutes = false;
#endif
    // Which day is it today.
    [SerializeField] private int day = 1;

    [Header("References")]
    [SerializeField] private Player player;
    [SerializeField] private Wall wall;

    float dayEnd = 0;

    private void Start() {

        Game.Assert(player != null, "Player was not assigned to GameManager!");
        Game.Assert(wall != null, "Wall was not assigned to GameManager!");

#if UNITY_EDITOR
        if (!dayLengthInMinutes)
            dayEnd = Time.time + dayLength;
        else
#endif
            dayEnd = Time.time + dayLength * 60;

    }
    private void Update() {

        if (Time.time < dayEnd) return;

        day++;
#if UNITY_EDITOR
        if (!dayLengthInMinutes)
            dayEnd = Time.time + dayLength;
        else
#endif
            dayEnd = Time.time + dayLength * 60;

    }

    public Player GetPlayer() { return GameManager.instance.player; }
    public Wall GetWall() { return GameManager.instance.wall; }

}