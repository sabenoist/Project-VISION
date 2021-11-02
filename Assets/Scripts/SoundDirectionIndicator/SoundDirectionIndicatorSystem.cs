using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDirectionIndicatorSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private SoundDirectionIndicator indicatorPrefab = null;

    [SerializeField]
    private RectTransform holder = null;

    [SerializeField]
    private new Camera camera = null;

    [SerializeField]
    private Transform player = null;

    private Dictionary<Transform, SoundDirectionIndicator> Indicators = new Dictionary<Transform, SoundDirectionIndicator>();

    #region Delegates
    public static Action<Transform, float, int, float> CreateIndicator = delegate { };
    public static Func<Transform, bool> CheckIfObjectInSight = null;
    #endregion

    private void OnEnable()
    {
        CreateIndicator += Create;
        CheckIfObjectInSight += InSight;
    }

    private void OnDisable()
    {
        CreateIndicator -= Create;
        CheckIfObjectInSight -= InSight;
    }

    public void Create(Transform target, float amplitude, int pitch, float button) // i added float button here 
    {
        if (Indicators.ContainsKey(target))
        {
            Indicators[target].Restart();
            return;
        }

        SoundDirectionIndicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, player, new Action( () => { Indicators.Remove(target); } ), amplitude, pitch, button); // i added button here 

        Indicators.Add(target, newIndicator);
    }

    public bool InSight(Transform t)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(t.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
