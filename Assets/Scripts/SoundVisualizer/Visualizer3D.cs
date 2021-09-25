using System.Collections.Generic;
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
    private List<GameObject> bars;

	/// <summary>
	/// Sets the respective 3d bar to the new given height.
	/// </summary>
	/// <param name="mic">Enum value of the microphone to determine which bar to change.</param>
	/// <param name="amplitude">The amplitude value to be determine the height of the bar.</param>
	public void SetBarHeight(Microphone mic, float amplitude) 
	{
		GameObject bar = bars[(int)mic];

		Vector3 newSize = bar.GetComponent<Transform>().localScale;
		newSize.y = amplitude;
		bar.GetComponent<Transform>().localScale = newSize;
	}

	/// <summary>
	/// Decays the height of each bar back to 0.
	/// </summary>
    public void FixedUpdate() 
	{
        foreach (GameObject bar in bars) 
		{
			Vector3 newSize = bar.GetComponent<Transform>().localScale;
			newSize.y = newSize.y - 0.25f;

			if (newSize.y < 0) 
			{
				newSize.y = 0;
            }

			bar.GetComponent<Transform>().localScale = newSize;
		}
    }

	/// <summary>
	/// Attaches an event listener to <see cref="MQTT_Client"/> for when new data arrives from the MQTT broker. 
	/// </summary>
    public void OnEnable() 
	{
		MQTT_Client.NetworkManager.AddReceivedDataListener(SetBarHeight);
    }

	/// <summary>
	/// Removes an event listener from <see cref="MQTT_Client"/> for when new data arrives from the MQTT broker.
	/// </summary>
	public void OnDisable() {
		MQTT_Client.NetworkManager.RemoveReceivedDataListener(SetBarHeight);
    }


}
