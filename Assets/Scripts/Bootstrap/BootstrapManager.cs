using System.Collections;
using System.IO;
using Showcase.PoolingSystem;
using Showcase.Providers;
using Showcase.Singleton;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Showcase.Bootstrap
{
    public class BootstrapManager : BaseSingleton<BootstrapManager>
    {
        private const string MainMenuScene = "MainMenu";
        private const float TransitionDelay = 1f;

        protected override void OnSingletonAwake()
        {
            Debug.Log("[Bootstrap] Bootstrap scene started");
            StartCoroutine(InitializationSequence());
        }

        private IEnumerator InitializationSequence()
        {
            ConfigProvider.Initialize();

            PoolManager.Initialize();

            // TODO: Initialize Pool System

            yield return new WaitForSeconds(TransitionDelay);
            LoadNextScene();
        }

        private void LoadNextScene()
        {
            if (!IsSceneInBuildSettings(MainMenuScene))
            {
                Debug.LogError($"[Bootstrap] Scene '{MainMenuScene}' not found in Build Settings!");
                return;
            }

            Debug.Log($"[Bootstrap] Loading scene: {MainMenuScene}");
            SceneManager.LoadScene(MainMenuScene);
        }

        private bool IsSceneInBuildSettings(string sceneName)
        {
            for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                string sceneNameFromPath = Path.GetFileNameWithoutExtension(scenePath);

                if (sceneNameFromPath == sceneName)
                    return true;
            }

            return false;
        }
    }
}