
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NewSoundOriginController : MonoBehaviour
{
	public Transform userTransform;
	public GameObject soundOriginPrefab;

	public float amplitudeScalar = 0.05f;
	public Material materialGreen;
	public Material materialBlue;
	public Material materialRed;
	/// <summary>
	/// Instantiates a SoundOrigin object based on the data provided by the dictionary parameter.
	/// </summary>
	/// <param name="data">Dictionary object holding the data sent from the server.</param>
	public void CreateSoundOrigin(Dictionary<string, string> data)
	{
		GameObject soundOrigin = Instantiate(soundOriginPrefab);
		IndicatorRegister indicatorRegister = soundOrigin.GetComponent<IndicatorRegister>();

		float decibels = getFloat(data, "decibels");
		int pitch = getInt(data, "pitch");
		float timePeriod = getFloat(data, "timeperiod");
		float distance = getFloat(data, "distance");
		float direction = getFloat(data, "direction");
		float button = getFloat(data, "button");
		float amplitude = getInt(data, "amplitude");

		if (decibels < 0 || pitch < 0 || timePeriod < 0 || distance < 0 || direction < 0 || button < 0)
		{
			Debug.Log("Incomplete data received.");
			return;
		}

		indicatorRegister.SetData(decibels, pitch, timePeriod, button);

		Vector3 newPosition = new Vector3(userTransform.position.x, userTransform.position.y, userTransform.position.z);
		soundOrigin.transform.position = newPosition;
		soundOrigin.transform.Translate(distance * Mathf.Cos(Mathf.PI / 180 * direction), 1, distance * Mathf.Sin(Mathf.PI / 180 * direction), 0f);
		//change sound origin size
		Vector3 newSize = soundOrigin.GetComponent<Transform>().localScale;
		//change size of the circle based on the  spectrumData
		newSize.x = amplitude * amplitudeScalar;
		newSize.y = amplitude * amplitudeScalar;
		newSize.z = amplitude * amplitudeScalar;
		soundOrigin.GetComponent<Transform>().localScale = newSize;
		if (pitch == 0)
		{
			//soundOrigin.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.green;
			soundOrigin.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = materialGreen;
		}
		else if (pitch == 1)
		{
			//soundOrigin.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
			soundOrigin.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = materialBlue;
		}
		else if (pitch >= 2)
		{
			//soundOrigin.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().color = Color.red;
			soundOrigin.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material = materialRed;
		}
		indicatorRegister.Register();
	}

	/// <summary>
	/// Attempts to extract a float value from the dictionary. If value is not found it will return -1.
	/// </summary>
	/// <param name="dic">Dictionary holding the key and value to extract.</param>
	/// <param name="key">The key to extract the data with.</param>
	/// <returns>A float value extracted from the dictionary. If value not found it will return -1.</returns>
	private float getFloat(Dictionary<string, string> dic, string key)
	{
		if (!dic.TryGetValue(key, out string value))
		{
			return -1;
		}
		return float.Parse(value);
	}

	/// <summary>
	/// Attempts to extract an int value from the dictionary. If value is not found it will return -1.
	/// </summary>
	/// <param name="dic">Dictionary holding the key and value to extract.</param>
	/// <param name="key">The key to extract the data with.</param>
	/// <returns>An int value extracted from the dictionary. If value not found it will return -1.</returns>
	private int getInt(Dictionary<string, string> dic, string key)
	{
		if (!dic.TryGetValue(key, out string value))
		{
			return -1;
		}
		return int.Parse(value);
	}

	/// <summary>
	/// Attaches an event listener to <see cref="MQTT_Client"/> for when new data arrives from the MQTT broker. 
	/// </summary>
	public void OnEnable()
	{
		StartCoroutine(SubscribeListeners());
	}

	/// <summary>
	/// Removes an event listener from <see cref="MQTT_Client"/> for when new data arrives from the MQTT broker.
	/// </summary>
	public void OnDisable()
	{
		if (MQTT_Client.NetworkManager != null)
		{
			MQTT_Client.NetworkManager.RemoveReceivedDataListener(CreateSoundOrigin);
		}
	}

	/// <summary>
	/// Pauses the execution of this script until the NetworkManager is ready.
	/// </summary>
	/// <param name="seconds">The amount of seconds the script gets paused.</param>
	/// <returns>Stuff needed for the Coroutine.</returns>
	private IEnumerator SubscribeListeners()
	{
		while (MQTT_Client.NetworkManager == null)
		{
			yield return new WaitForSeconds(1);
		}

		MQTT_Client.NetworkManager.AddReceivedDataListener(CreateSoundOrigin);
	}
}
