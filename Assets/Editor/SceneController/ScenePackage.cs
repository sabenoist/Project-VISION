using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Holds the scenes that belong to each other.
/// </summary>
[CreateAssetMenu(fileName = "ScenePackage", menuName = "Scenes/Create ScenePackage", order = 1)]
public class ScenePackage : ScriptableObject {
    /// <summary>
    /// The name of the scene.
    /// </summary>
    [field: SerializeField]
    public SceneName Name { get; private set; }

    /// <summary>
    /// Holds the scenes that belong to each other.
    /// </summary>
    [field: SerializeField]
    public List<SceneAsset> Scenes { get; private set; }
}
