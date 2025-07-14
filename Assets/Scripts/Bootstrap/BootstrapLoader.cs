using UnityEngine;
using UnityEngine.SceneManagement;

namespace Showcase.Bootstrap
{
    /// <summary>
    /// Forces the game to always start from Bootstrap scene,
    /// even if we hit Play on a different scene in the editor
    /// </summary>
    public static class BootstrapLoader
    {
        private const string BootstrapSceneName = "Bootstrap";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadBootstrap()
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == BootstrapSceneName)
            {
                Debug.Log("Already in Bootstrap scene");
                return;
            }

            Debug.Log($"Started in {currentScene} scene, but redirecting to Bootstrap");

            SceneManager.LoadScene(BootstrapSceneName);
        }
    }
}