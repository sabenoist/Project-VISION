using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorRegister : MonoBehaviour
{
    [Range(5, 30)]
    [SerializeField] 
    private float destroyTimer = 15.0f;
    [SerializeField]
    private float amplitude = 10.0f;
    [SerializeField]
    private int pitch = 1;

    void OnEnable()
    {
        //Invoke("Register", Random.Range(0, 8));
        //Invoke("Register", 1.0f);
    }

    public void SetData(float amplitude, int pitch, float destroyTimer) {
        this.amplitude = amplitude;
        this.pitch = pitch;
        this.destroyTimer = destroyTimer;
    }

    public void Register()
    {
        if (!SoundDirectionIndicatorSystem.CheckIfObjectInSight(this.transform))
        {
            SoundDirectionIndicatorSystem.CreateIndicator(this.transform, amplitude, pitch);
        }

        Destroy(this.gameObject, destroyTimer);
    }

}
