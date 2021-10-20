using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleAnimation : MonoBehaviour
{
	//public float minHeight = 15.0f;
	//public float maxHeight = 425.0f;
	//public float updateSentivity = 10.0f;
	//public Color visualizerColor = Color.gray;
	//[Space(15)]
	//public AudioClip audioClip;
	//public bool loop = true;
	//[Space(15), Range(64, 8192)]
	//public int visualizerSimples = 64;

	public float[] spectrumData;
	//public float spectrumData = 25.0f;
	public GameObject[] visualizerObjects;
	//AudioSource audioSource;

	// Use this for initialization
	void Start()
	{
		/*visualizerObjects = GetComponentsInChildren<VisualizerObjectScript>();

		if (!audioClip)
			return;

		audioSource = new GameObject("_AudioSource").AddComponent<AudioSource>();
		audioSource.loop = loop;
		audioSource.clip = audioClip;
		audioSource.Play();*/
	}

	// Update is called once per frame
	void FixedUpdate()
	{
		//float[] spectrumData = audioSource.GetSpectrumData(visualizerSimples, 0, FFTWindow.Rectangular);

		for (int i = 0; i < visualizerObjects.Length; i++)
		{
			//RectTransform rectTransform = visualizerObjects[i].GetComponent<RectTransform>();
			//rectTransform.sizeDelta = new Vector2(spectrumData, spectrumData);        //newSize.y = Mathf.Clamp(Mathf.Lerp(newSize.y, minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5.0f), updateSentivity * 0.5f), minHeight, maxHeight);

			Vector3 newSize = visualizerObjects[i].GetComponent<Transform>().localScale;

			if (spectrumData[i]>5)
            {
				//newSize.y = Mathf.Clamp(Mathf.Lerp(newSize.y, minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5.0f), updateSentivity * 0.5f), minHeight, maxHeight);
				newSize.y = spectrumData[i];
				newSize.x = spectrumData[i];
				visualizerObjects[i].GetComponent<Transform>().localScale = newSize;
				//visualizerObjects [i].GetComponent<Image> ().color = visualizerColor;

				StartCoroutine(ShowAndHide(0.5f));
			}

			else
            {
				//filter out sound under certain volumn
				visualizerObjects[i].SetActive(false);
			}
		}
	}

	IEnumerator ShowAndHide(float delay)
	{
		for (int i = 0; i < visualizerObjects.Length; i++)//visualizerObjects.SetActive(true
		{
			yield return new WaitForSeconds(delay);
			visualizerObjects[i].SetActive(false);
		}
	}
}
