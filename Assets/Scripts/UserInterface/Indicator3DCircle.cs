using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Indicator3DCircle : MonoBehaviour
{
	public GameObject circleRed; //prefab to be Instantiated, Red for high pitch
	public GameObject circleBlue;//blue for medium pitch
	public GameObject circleGreen;//green for low pitch
	public GameObject[] CircleGroup; //store the instantiated prefab

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

			if (pitch[i] == "2" && !CircleGroup[i])
			{
				//instantiate circle prefab and specify position with Vector3
				CircleGroup[i] = Instantiate(circleRed, position[i], Quaternion.identity) as GameObject;


				Vector3 newSize = circleRed.GetComponent<Transform>().localScale;
				//change size of the circle based on the  spectrumData
				newSize.y = volumnData[i];
				newSize.x = volumnData[i];
				circleRed.GetComponent<Transform>().localScale = newSize;

				//slowly fade out the circle
				StartCoroutine(FadeOutMaterial(1.5f, CircleGroup[i]));
				Destroy(CircleGroup[i], 1.5f);//destory circle prefab after 1.5 seconds

				//remove used spectrumData from array to prevent it instantiate prefab again
				StartCoroutine(ShowAndHide(0.6f));
			}

			//pitch represent color and volumn represent size
			else if (pitch[i] == "1" && !CircleGroup[i])
			{
				//instantiate circle prefab and specify position with Vector3
				//receive position value from server
				CircleGroup[i] = Instantiate(circleBlue, position[i], Quaternion.identity) as GameObject;


				Vector3 newSize = circleBlue.GetComponent<Transform>().localScale;
				//change size of the circle based on the  spectrumData
				newSize.y = volumnData[i];
				newSize.x = volumnData[i];
				circleBlue.GetComponent<Transform>().localScale = newSize;
				StartCoroutine(FadeOutMaterial(1.5f, CircleGroup[i]));
				Destroy(CircleGroup[i], 1.5f);//destory circle prefab  after 1.5 seconds

				//remove used spectrumData from array to prevent it instantiate prefab again
				StartCoroutine(ShowAndHide(0.6f));
			}

			else if (pitch[i] == "0" && !CircleGroup[i])
			{
				//instantiate circle prefab and specify position with Vector3
				CircleGroup[i] = Instantiate(circleGreen, position[i], Quaternion.identity) as GameObject;

				Vector3 newSize = circleGreen.GetComponent<Transform>().localScale;
				//change size of the circle based on the  spectrumData
				newSize.y = volumnData[i];
				newSize.x = volumnData[i];
				circleGreen.GetComponent<Transform>().localScale = newSize;
				StartCoroutine(FadeOutMaterial(1.5f, CircleGroup[i]));
				Destroy(CircleGroup[i], 1.5f);//destory circle prefab after 1.5 seconds

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
	IEnumerator FadeOutMaterial(float fadeSpeed, GameObject circle)
	{

		Renderer rend = circle.transform.GetComponent<Renderer>();
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