using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Bar3D : MonoBehaviour
{
	public GameObject barRed; //prefab to be Instantiated, Red for high pitch
	public GameObject barBlue;//blue for medium pitch
	public GameObject barGreen;//green for low pitch
	public GameObject[] barGroup; //store the instantiated prefab

	public float[] volumnData;//to be received from the server
	public string[] pitch; //array of pitch to be received from servcer
	public Vector3[] position;////to be received from the server

	float appearing_speed = 2f;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void FixedUpdate()
	{
		for (int i = 0; i < volumnData.Length; i++)
		{

			if (pitch[i] == "2" && !barGroup[i])
			{
				//instantiate circle prefab and specify position with Vector3
				barGroup[i] = Instantiate(barRed, position[i], Quaternion.identity) as GameObject;


				Vector3 newSize = barRed.GetComponent<Transform>().localScale;
				//change size of the circle based on the  spectrumData
				newSize.y = volumnData[i];
			//newSize.x = volumnData[i];
			barRed.GetComponent<Transform>().localScale = newSize;

				//slowly fade out the circle
				StartCoroutine(FadeOutMaterial(1.5f, barGroup[i]));
				Destroy(barGroup[i], 1.5f);//destory circle prefab after 1.5 seconds

				//remove used spectrumData from array to prevent it instantiate prefab again
				StartCoroutine(ShowAndHide(0.6f));
			}

			//pitch represent color and volumn represent size
			else if (pitch[i] == "1" && !barGroup[i])
			{
			//instantiate circle prefab and specify position with Vector3
			//receive position value from server
			barGroup[i] = Instantiate(barBlue, position[i], Quaternion.identity) as GameObject;


				Vector3 newSize = barBlue.GetComponent<Transform>().localScale;
				//change size of the circle based on the  spectrumData
				newSize.y = volumnData[i];
			//newSize.x = volumnData[i];
			barBlue.GetComponent<Transform>().localScale = newSize;
				StartCoroutine(FadeOutMaterial(1.5f, barGroup[i]));
				Destroy(barGroup[i], 1.5f);//destory circle prefab  after 1.5 seconds

				//remove used spectrumData from array to prevent it instantiate prefab again
				StartCoroutine(ShowAndHide(0.6f));
			}

			else if (pitch[i] == "0" && !barGroup[i])
			{
			//instantiate circle prefab and specify position with Vector3
			barGroup[i] = Instantiate(barGreen, position[i], Quaternion.identity) as GameObject;

				Vector3 newSize = barGreen.GetComponent<Transform>().localScale;
				//change size of the circle based on the  spectrumData
				newSize.y = volumnData[i];
			//newSize.x = volumnData[i];
			barGreen.GetComponent<Transform>().localScale = newSize;
				StartCoroutine(FadeOutMaterial(1.5f, barGroup[i]));
				Destroy(barGroup[i], 1.5f);//destory circle prefab after 1.5 seconds

				//remove used spectrumData from array to prevent it instantiate prefab again
				StartCoroutine(ShowAndHide(0.6f));

			}


		}
	}
	IEnumerator ShowAndHide(float delay)
	{
		for (int i = 0; i < volumnData.Length; i++)
		{
			yield return new WaitForSeconds(delay);
			volumnData = volumnData.Skip(i).ToArray();
		}


	}
	//slowly fade out the circle
	IEnumerator FadeOutMaterial(float fadeSpeed, GameObject bar)
	{

		Renderer rend = bar.transform.GetComponent<Renderer>();
		Color matColor = rend.material.color;
		float alphaValue = rend.material.color.a;

		while (rend.material.color.a > 0f)
		{
			alphaValue -= Time.deltaTime / fadeSpeed;
			rend.material.color = new Color(matColor.r, matColor.g, matColor.b, alphaValue);
			yield return null;
		}
		rend.material.color = new Color(matColor.r, matColor.g, matColor.b, 0f);
	}
}