using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum for the different microphones.
/// </summary>
public enum Microphone { MIC0, MIC1, MIC2, MIC3 }

/// <summary>
/// Class to handle the visualization of the 3d bars.
/// </summary>
public class Visualizer : MonoBehaviour
{
	/// <summary>
	/// Sets the threshold for ignoring messages that fall below it.
	/// </summary>
	[SerializeField]
	private float threshold = 15;

	/// <summary>
	/// Sets the decay rate of the bars.
	/// </summary>
	[SerializeField]
	private float decay = 2.5f;

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
		if (amplitude < threshold) {
			return;
        }

		GameObject bar = bars[(int)mic];
		RectTransform rectTransform = bar.GetComponent<RectTransform>();

		if (rectTransform.sizeDelta.y < amplitude) {
			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, amplitude);
		}
	}

	/// <summary>
	/// Decays the height of each bar back to 0.
	/// </summary>
    public void FixedUpdate() 
	{
        foreach (GameObject bar in bars) 
		{
			RectTransform rectTransform = bar.GetComponent<RectTransform>();
			float newHeight = rectTransform.sizeDelta.y - decay;

			if (newHeight < 0) 
			{
				newHeight = 0;
            }

			rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, newHeight);
		}
    }

	/// <summary>
	/// Attaches an event listener to <see cref="MQTT_Client"/> for when new data arrives from the MQTT broker. 
	/// </summary>
    public void OnEnable() 
	{
		if (MQTT_Client.NetworkManager != null) { 
			MQTT_Client.NetworkManager.AddReceivedDataListener(SetBarHeight);
		}
		else {
			Debug.Log("Connection to MQTT client could not be found. Aborting adding listener.");
        }
	}

	/// <summary>
	/// Removes an event listener from <see cref="MQTT_Client"/> for when new data arrives from the MQTT broker.
	/// </summary>
	public void OnDisable() {
		if (MQTT_Client.NetworkManager != null) {
			MQTT_Client.NetworkManager.RemoveReceivedDataListener(SetBarHeight);
		}
    }
}
