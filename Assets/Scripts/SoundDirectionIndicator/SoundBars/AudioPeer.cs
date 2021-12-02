using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AudioSource))]
public class AudioPeer : MonoBehaviour
{
    private AudioSource audioSource;

    public float scalar = 1;
    public float[] samples = new float[64];
    public GameObject[] bars = new GameObject[8];

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        GetSpectrumAudioSource();

        for (int i = 0; i < bars.Length; i++)
        {
            Vector3 bar = bars[i].transform.localScale;

            if (i == 0)
            {
                bars[i].transform.localScale = new Vector3(bar.x, samples[0] * scalar, bar.z);
            }
            else
            {
                bars[i].transform.localScale = new Vector3(bar.x, samples[i * 8 - 1] * scalar, bar.z);
            }
            
        }
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }
}
