using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Visualizer3D : MonoBehaviour
{
	public float spectrumData = 15.0f;//data need to be received 
    public GameObject visualizerObject;
	

	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void FixedUpdate()
	{
			Vector3 newSize = visualizerObject.GetComponent<Transform>().localScale;		
			newSize.y =  spectrumData;
			visualizerObject.GetComponent<Transform>().localScale = newSize;
		
	}
}
