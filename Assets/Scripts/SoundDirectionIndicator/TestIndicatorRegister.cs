using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestIndicatorRegister : MonoBehaviour
{
    [Range(5, 30)]
    [SerializeField] 
    float destroyTimer = 15.0f;

    void Start()
    {
        Invoke("Register", Random.Range(0, 8));
    }

    public void Register()
    {
        if (!SoundDirectionIndicatorSystem.CheckIfObjectInSight(this.transform))
        {
            SoundDirectionIndicatorSystem.CreateIndicator(this.transform);
        }

        Destroy(this.gameObject, destroyTimer);
    }

}
