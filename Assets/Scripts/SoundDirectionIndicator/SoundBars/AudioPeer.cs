using System;
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
                if (samples[0] * scalar > 14)
                {
                    samples[0] = 14 / scalar;
                }
                bars[i].transform.localScale = new Vector3(bar.x, samples[0] * scalar, bar.z);
                //bars[i].transform.localScale = new Vector3(bar.x, (float)Math.Log(samples[0]), bar.z);
            }
            else
            {
                if (samples[i * 8 - 1] * scalar > 14)
                {
                    samples[i * 8 - 1] = 14 / scalar;
                }
                bars[i].transform.localScale = new Vector3(bar.x, samples[i * 8 - 1] * scalar, bar.z);
                //bars[i].transform.localScale = new Vector3(bar.x, (float)Math.Log(samples[i * 8 - 1]), bar.z);
            }
            
        }
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }
}
