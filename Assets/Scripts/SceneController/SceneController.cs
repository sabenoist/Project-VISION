using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene loading and unloading.
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// The name of the scene package to start with.
    /// </summary>
    [SerializeField]
    private SceneName startingScene;

    /// <summary>
    /// Holds the scene packages to be used.
    /// </summary>
    [SerializeField]
    private List<ScenePackage> scenePackages;

    /// <summary>
    /// Executes at the start to load in the starting scene.
    /// </summary>
    void Start()
    {
        loadScenePackage(startingScene);
    }

    /// <summary>
    /// Loads in a scene package.
    /// </summary>
    /// <param name="packageName">The index of the <see cref="SceneName"/> of the package to load in.</param>
    public void loadScenePackage(SceneName packageName) {
        ScenePackage package = findScenePackage(packageName);
        if (package == null) {
            return;
        }

        UnloadScenes();
        
        foreach(string scene in package.Scenes) {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
        }
        
    }

    /// <summary>
    /// Unloads all scene but the main scene.
    /// </summary>
    private void UnloadScenes() {
        for (int i = 1; i < SceneManager.sceneCount; i++) {
            SceneManager.UnloadSceneAsync(i);
        }
    }

    /// <summary>
    /// Returns a package based on its <see cref="SceneName"/> index. Returns null if the package could not be found.
    /// </summary>
    /// <param name="name">The <see cref="SceneName"/> index.</param>
    /// <returns></returns>
    private ScenePackage findScenePackage(SceneName name) {
        foreach (ScenePackage package in scenePackages) {
            if (package.Name == name) {
                return package;
            }
        }

        return null;
    }

}
