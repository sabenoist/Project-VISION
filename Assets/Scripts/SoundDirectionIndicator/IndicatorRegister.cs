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
    [SerializeField]
    private float button = 1;

    //adjust this to change speed
    public float speed = 5f;
    //adjust this to change how high it goes
    public float height = 0.5f;

    /// <summary>
    /// Handles the floating of the arrow.
    /// </summary>
    private void Update()
    {
        Vector3 pos = transform.position;
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        transform.position = new Vector3(pos.x, newY, pos.z);
    }

    void OnEnable()
    {
        //Invoke("Register", Random.Range(0, 8));
        //Invoke("Register", 1.0f);
    }

    public void SetData(float amplitude, int pitch, float destroyTimer, float button) {
        this.amplitude = amplitude;
        this.pitch = pitch;
        this.button = button;
        this.destroyTimer = destroyTimer;
    }

    public void Register()
    {
        if (!SoundDirectionIndicatorSystem.CheckIfObjectInSight(this.transform))
        {
            SoundDirectionIndicatorSystem.CreateIndicator(this.transform, amplitude, pitch, button, destroyTimer);
        }

        Destroy(this.gameObject, destroyTimer);
    }

}
