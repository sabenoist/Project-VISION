using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

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
    public static Action<Transform, float, int, float, float> CreateIndicator = delegate { };
    public static Func<Transform, bool> CheckIfObjectInSight = null;
    #endregion

    private void Update()
    {
        //holder.rotation = Quaternion.Euler(holder.rotation.x, holder.rotation.y, camera.transform.rotation.z * 3600f);
        //this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, camera.transform.rotation.z * 3600f);
        //holder.gameObject.transform.rotation = Quaternion.Euler(holder.gameObject.transform.rotation.x, camera.transform.rotation.y * 3600f, holder.gameObject.transform.rotation.z);

        Vector3 eulerRotation = new Vector3(holder.gameObject.transform.eulerAngles.x, holder.transform.eulerAngles.y, camera.transform.eulerAngles.y);
        holder.gameObject.transform.rotation = Quaternion.Euler(eulerRotation);
    }

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

    public void Create(Transform target, float amplitude, int pitch, float button, float destroyTimer)
    {
        if (Indicators.ContainsKey(target))
        {
            Indicators[target].Restart();
            return;
        }

        SoundDirectionIndicator newIndicator = Instantiate(indicatorPrefab, holder);
        newIndicator.Register(target, player, new Action( () => { Indicators.Remove(target); } ), amplitude, pitch, button, destroyTimer); 

        Indicators.Add(target, newIndicator);
    }

    public bool InSight(Transform t)
    {
        Vector3 screenPoint = camera.WorldToViewportPoint(t.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }
}
