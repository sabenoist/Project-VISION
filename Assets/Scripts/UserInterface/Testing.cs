using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class Testing : MonoBehaviour
{
	
	public GameObject[] CircleGroup; //store the instantiated prefab
	public GameObject arrowRed;//blue for medium pitch
	public GameObject arrowBlue;
	public GameObject arrowGreen;

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
				CircleGroup[i] = Instantiate(arrowRed, position[i], Quaternion.identity) as GameObject;
				//instantiate prefab as child of CircleCanvas
				CircleGroup[i].transform.SetParent(GameObject.FindGameObjectWithTag("CircleCanvas").transform, false);
				GameObject indicator = CircleGroup[i].transform.GetChild(0).gameObject;

				Vector3 newSize = indicator.GetComponent<Transform>().localScale;
				//change size of the circle based on the  spectrumData
				newSize.y = volumnData[i];
				//newSize.x = volumnData[i];
				indicator.GetComponent<Transform>().localScale =newSize;

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
				CircleGroup[i] = Instantiate(arrowBlue, position[i], Quaternion.identity) as GameObject;
				//instantiate prefab as child of CircleCanvas
				CircleGroup[i].transform.SetParent(GameObject.FindGameObjectWithTag("CircleCanvas").transform, false);
				GameObject indicator = CircleGroup[i].transform.GetChild(0).gameObject;

				Vector3 newSize = indicator.GetComponent<Transform>().localScale;
				//change size of the circle based on the  spectrumData
				newSize.y = volumnData[i];
				//newSize.x = volumnData[i];
				indicator.GetComponent<Transform>().localScale =newSize;

			    //slowly fade out the circle
				StartCoroutine(FadeOutMaterial(1.5f, CircleGroup[i]));
				Destroy(CircleGroup[i], 1.5f);//destory circle prefab after 1.5 seconds

				//remove used spectrumData from array to prevent it instantiate prefab again
			    StartCoroutine(ShowAndHide(0.6f));
			}

			else if (pitch[i] == "0" && !CircleGroup[i])
			{
				//instantiate circle prefab and specify position with Vector3
				CircleGroup[i] = Instantiate(arrowGreen, position[i], Quaternion.identity) as GameObject;
				//instantiate prefab as child of CircleCanvas
				CircleGroup[i].transform.SetParent(GameObject.FindGameObjectWithTag("CircleCanvas").transform, false);
				GameObject indicator = CircleGroup[i].transform.GetChild(0).gameObject;

				Vector3 newSize = indicator.GetComponent<Transform>().localScale;
				//change size of the circle based on the  spectrumData
				newSize.y = volumnData[i];
				//newSize.x = volumnData[i];
				indicator.GetComponent<Transform>().localScale =newSize;

			    //slowly fade out the circle
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
		RawImage image = circle.transform.GetComponent<RawImage>();
		Color oldColor = image.color;
		float alphaValue = image.color.a;

		while (image.color.a > 0f)
		{
			alphaValue -= Time.deltaTime / fadeSpeed;
			image.color = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
			yield return null;
		}
		image.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0f);

	}
}
