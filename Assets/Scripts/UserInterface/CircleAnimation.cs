using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class CircleAnimation : MonoBehaviour
{


	public float[] volumnData;//to be accepted from the server
	
	public GameObject circlePrefab; //prefab to be Instantiated
	public GameObject[] CircleGroup; //store the instantiated prefab


	// Use this for initialization
	void Start()
	{
		
	}

	// Update is called once per frame
	void FixedUpdate()
	{
	
		for (int i = 0; i < volumnData.Length; i++)
		{
		
			//filter out sound below certain volumn
			if (volumnData[i] >2 & !CircleGroup[i])
            {

				//instantiate circle prefab and specify position with Vector3
				CircleGroup[i]=Instantiate(circlePrefab, new Vector3(0,0,0), Quaternion.identity) as GameObject;
				Vector3 newSize = circlePrefab.GetComponent<Transform>().localScale;
				//change size of the circle based on the  spectrumData
				newSize.y = volumnData[i];
				newSize.x = volumnData[i];
				circlePrefab.GetComponent<Transform>().localScale = newSize;
				
				Destroy(CircleGroup[i], 1);//destory circle after 2 seconds

				//remove used spectrumData from array to prevent it instantiate prefab again
				StartCoroutine(ShowAndHide(0.5f));

			}
			
			/*if ()
			{
				//filter out sound under certain volumn
				circlePrefab.SetActive(false);
			}*/
		}
	}

	IEnumerator ShowAndHide(float delay)
	{
		for(int i = 0; i < volumnData.Length; i++)
        {
			yield return new WaitForSeconds(delay);
			volumnData = volumnData.Skip(i).ToArray();
		}
			
		
	}
}
