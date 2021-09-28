using UnityEngine;

/// <summary>
/// Button to generate random amplitudes to test visualization by the Visualizer3D script.
/// </summary>
public class RandomSoundButton : MonoBehaviour
{
    /// <summary>
    /// Reference to the Visualizer3D script.
    /// </summary>
    [SerializeField]
    private Visualizer visualizerScript;

    /// <summary>
    /// The maximum value that can be randomly generated.
    /// </summary>
    [SerializeField]
    private float maxRange;

    /// <summary>
    /// Generates random values in range of the <see cref="maxRange"/> for the 3d bars of the <see cref="Visualizer"/> object.
    /// </summary>
    public void GenerateRandomValues() {
        visualizerScript.SetBarHeight(Microphone.MIC0, Random.Range(0f, maxRange));
        visualizerScript.SetBarHeight(Microphone.MIC1, Random.Range(0f, maxRange));
        visualizerScript.SetBarHeight(Microphone.MIC2, Random.Range(0f, maxRange));
        visualizerScript.SetBarHeight(Microphone.MIC3, Random.Range(0f, maxRange));
    }
}
