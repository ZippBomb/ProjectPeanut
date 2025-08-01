using UnityEngine;

public static class Game {

    public static MainInput input;

    public static void Start() {

        input = new MainInput();

    }

    public static void Assert(bool condition, string message) {

#if UNITY_EDITOR
        if (condition) return;

        Debug.LogError(message);
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

}