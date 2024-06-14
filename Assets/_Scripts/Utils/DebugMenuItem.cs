using UnityEngine;
using UnityEditor;
using System.IO;

#if UNITY_EDITOR
public class DebugMenuItem : MonoBehaviour
{
    [MenuItem("DebugMenu/DeleteAll")]
    public static void DeleteData()
    {
        File.Delete(Path.Combine(Application.persistentDataPath, $"{typeof(GameData)}.json"));
    }
    [MenuItem("DebugMenu/Restart")]
    public static void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
#endif