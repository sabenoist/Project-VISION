using UnityEngine;

/// <summary>
/// Enum for the different microphones.
/// </summary>
public enum Microphone { MIC0, MIC1, MIC2, MIC3 }

/// <summary>
/// Class to handle the visualization of the 3d bars.
/// </summary>
public class Visualizer3D : MonoBehaviour
{
	/// <summary>
	/// Reference to the game object representing the first 3d bar.
	/// </summary>
	[SerializeField]
    private GameObject Bar0;

	/// <summary>
	/// Reference to the game object representing the second 3d bar.
	/// </summary>
	[SerializeField]
	private GameObject Bar1;

	/// <summary>
	/// Reference to the game object representing the third 3d bar.
	/// </summary>
	[SerializeField]
	private GameObject Bar2;

	/// <summary>
	/// Reference to the game object representing the fourth 3d bar.
	/// </summary>
	[SerializeField]
	private GameObject Bar3;

	/// <summary>
	/// Sets the respective 3d bar to the new given height.
	/// </summary>
	/// <param name="mic">Enum value of the microphone to determine which bar to change.</param>
	/// <param name="amplitude">The amplitude value to be determine the height of the bar.</param>
	public void SetBarHeight(Microphone mic, float amplitude) 
	{
		GameObject visualizerObject;

		switch (mic) 
		{
			case Microphone.MIC0:
				visualizerObject = Bar0;
				break;
			case Microphone.MIC1:
				visualizerObject = Bar1;
				break;
			case Microphone.MIC2:
				visualizerObject = Bar2;
				break;
			case Microphone.MIC3:
				visualizerObject = Bar3;
				break;
			default:
				return;
		}

		Vector3 newSize = visualizerObject.GetComponent<Transform>().localScale;
		newSize.y = amplitude;
		visualizerObject.GetComponent<Transform>().localScale = newSize;
	}
}
