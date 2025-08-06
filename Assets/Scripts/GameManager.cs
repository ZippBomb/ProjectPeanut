using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    private void Awake() {

        instance = this;
        Game.Start();

    }

    [SerializeField] private Wall wall;

    private void Start() {

        Game.Assert(wall != null, "Wall was not assigned to GameManager!");

    }

    public Wall GetWall() { return GameManager.instance.wall; }

}