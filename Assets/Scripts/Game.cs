using UnityEngine;

public static class Game {

    public static bool inMainMenu = true;

    public static MainInput input;

    public static void Start() {

        input = new MainInput();
        if (!inMainMenu) {

            input.Enable();

        }

    }

    public static void DisableInventoryInput() {

        input.Items.Disable();

    }
    public static void DisableInteractionInput() {

        input.Player.Interact.Disable();

    }

    public static void EnableInventoryInput() {

        input.Items.Enable();

    }
    public static void EnableInteractionInput() {

        input.Player.Interact.Enable();

    }

    public static void Assert(bool condition, string message) {

#if UNITY_EDITOR
        if (condition) return;

        Debug.LogError(message);
        UnityEditor.EditorApplication.isPlaying = false;
#endif

    }

    public static void Quit() {
        
        Debug.Log("Quitting game.");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

    }

}
