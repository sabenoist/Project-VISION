using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles scene loading and unloading.
/// </summary>
public class SceneController : MonoBehaviour
{
    /// <summary>
    /// Holds the scene packages to be used.
    /// </summary>
    [SerializeField]
    private List<ScenePackage> scenes;

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
