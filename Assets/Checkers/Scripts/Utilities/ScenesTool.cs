#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ScenesTool
{
    [MenuItem("Scenes/Checkers")]
    public static void Preloader()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/Checkers/Scenes/" + "Checkers" + ".unity");
    }

    [MenuItem("Scenes/PuzzleEditor")]
    public static void GameStart()
    {
        EditorSceneManager.OpenScene(Application.dataPath + "/Checkers/Scenes/" + "PuzzleEditor" + ".unity");
    }
}
#endif