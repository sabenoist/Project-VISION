using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOriginController : MonoBehaviour
{
	public GameObject soundOriginPrefab;

	public void CreateSoundOrigin(Dictionary<string, string> data) {
		GameObject soundOrigin = Instantiate(soundOriginPrefab);
		IndicatorRegister indicatorRegister = soundOrigin.GetComponent<IndicatorRegister>();
		indicatorRegister.SetData(float.Parse(data["decibels"]), int.Parse(data["pitch"]), float.Parse(data["timeperiod"]));
	}

	/// <summary>
	/// Attaches an event listener to <see cref="MQTT_Client"/> for when new data arrives from the MQTT broker. 
	/// </summary>
	public void OnEnable() {
		StartCoroutine(SubscribeListeners());
	}

	/// <summary>
	/// Removes an event listener from <see cref="MQTT_Client"/> for when new data arrives from the MQTT broker.
	/// </summary>
	public void OnDisable() {
		if (MQTT_Client.NetworkManager != null) {
			MQTT_Client.NetworkManager.RemoveReceivedDataListener(CreateSoundOrigin);
		}
	}

	/// <summary>
	/// Pauses the execution of this script until the NetworkManager is ready.
	/// </summary>
	/// <param name="seconds">The amount of seconds the script gets paused.</param>
	/// <returns>Stuff needed for the Coroutine.</returns>
	private IEnumerator SubscribeListeners() {
		while (MQTT_Client.NetworkManager == null) {
			yield return new WaitForSeconds(1);
		}

		MQTT_Client.NetworkManager.AddReceivedDataListener(CreateSoundOrigin);
	}
}
