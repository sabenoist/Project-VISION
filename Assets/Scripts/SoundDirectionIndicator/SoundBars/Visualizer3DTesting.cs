using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer3DTesting : MonoBehaviour
{
	//public float minHeight = 15.0f;
	//public float maxHeight = 425.0f;
	//public float updateSentivity = 10.0f;
	//public Color visualizerColor = Color.gray;
	[Space(15)]
	public AudioClip audioClip;
	//public bool loop = true;
	[Space(15), Range(64, 8192)]
	//public int visualizerSimples = 64;
	public float[] spectrumData = new float[4];
	VisualizerObjectScript[] visualizerObjects;
	AudioSource audioSource;

	// Use this for initialization
	void Start()
	{
		visualizerObjects = GetComponentsInChildren<VisualizerObjectScript>();
		/*
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

			Vector3 newSize = visualizerObjects[i].GetComponent<Transform>().localScale;
			//newSize.y = Mathf.Clamp(Mathf.Lerp(newSize.y, minHeight + (spectrumData[i] * (maxHeight - minHeight) * 5.0f), updateSentivity * 0.5f), minHeight, maxHeight);
			newSize.y =  spectrumData[i];
			visualizerObjects[i].GetComponent<Transform>().localScale = newSize;
			//visualizerObjects [i].GetComponent<Image> ().color = visualizerColor;
		}
	}
}
