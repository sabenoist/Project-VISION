using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SoundDirectionIndicator : MonoBehaviour 
{
    [SerializeField]
    private const float MaxTimer = 8.0f;
    private float timer = MaxTimer;

    private CanvasGroup canvasGroup = null;
    protected CanvasGroup CanvasGroup 
    {
        get 
        {
            if (canvasGroup == null) 
            {
                canvasGroup = GetComponent<CanvasGroup>();
                if (canvasGroup == null) 
                {
                    canvasGroup = gameObject.AddComponent<CanvasGroup>();
                }
            }
            return canvasGroup;
        }
    }

    private RectTransform rect = null;
    protected RectTransform Rect
    {
        get
        {
            if (rect == null)
            {
                rect = GetComponent<RectTransform>();
                if (rect == null)
                {
                    rect = gameObject.AddComponent<RectTransform>();
                }
            }
            return rect;
        }
    }

    public Transform Target { get; protected set; } = null;
    private Transform player = null;

    private IEnumerator IE_Countdown = null;
    private Action unRegister = null;

    private Quaternion targetRotation = Quaternion.identity;
    private Vector3 targetPosition = Vector3.zero;

    public GameObject circleRed; //prefab to be Instantiated, Red for high pitch
    public GameObject circleBlue;//blue for medium pitch
    public GameObject circleGreen;//green for low pitch

    private float amplitude;//to be received from the server
    private int pitch; //array of pitch to be received from servcer

    private GameObject circleVisualizer;

    public void Register(Transform target, Transform player, Action unRegister, float amplitude, int pitch)
    {
        this.amplitude = amplitude;
        this.pitch = pitch;
        this.Target = target;
        this.player = player;
        this.unRegister = unRegister;

        VisualizeCircle(amplitude, pitch);
        StartCoroutine(RotateToTheTarget());
        StartTimer();
    }

    public void Restart()
    {
        timer = MaxTimer;
        StartTimer();
    }

    private void StartTimer()
    {
        if (IE_Countdown != null) { StopCoroutine(IE_Countdown); }
        IE_Countdown = Countdown();
        StartCoroutine(IE_Countdown);
    }

    public IEnumerator RotateToTheTarget()
    {
        while (enabled)
        {
            if (Target)
            {
                targetPosition = Target.position;
                targetRotation = Target.rotation;
            }
            Vector3 direction = player.position - targetPosition;

            targetRotation = Quaternion.LookRotation(direction);
            targetRotation.z = -targetRotation.y;
            targetRotation.x = 0;
            targetRotation.y = 0;

            Vector3 NorthDirection = new Vector3(0, 0, player.eulerAngles.y);
            Rect.localRotation = targetRotation * Quaternion.Euler(NorthDirection);

            yield return null;
        }
    }

    private IEnumerator Countdown()
    {
        // visualize pointer
        while (CanvasGroup.alpha < 1.0f) {
            CanvasGroup.alpha += 4 * Time.deltaTime;
            yield return null;
        }

        while (timer > 0)
        {
            timer--;
            yield return new WaitForSeconds(1);
        }

        // unvisualize pointer
        while (CanvasGroup.alpha > 0.0f)
        {
            CanvasGroup.alpha -= 2 * Time.deltaTime;
            yield return null;
        }

        unRegister();
        Destroy(gameObject);
    }

    private void VisualizeCircle(float amplitude, int pitch) {
        if (pitch ==2)
        {
            circleVisualizer = Instantiate(circleRed, targetPosition, Quaternion.identity) as GameObject;
            circleVisualizer.transform.SetParent(GameObject.FindGameObjectWithTag("SoundDirectionHolder").transform, false);
            Vector3 newSize = circleRed.GetComponent<Transform>().localScale;
            //change size of the circle based on the  spectrumData
            newSize.y = amplitude* (float)0.05;
            newSize.x = amplitude* (float)0.05;
            circleRed.GetComponent<Transform>().localScale = newSize;
        }
        else if (pitch ==1)
        {
            circleVisualizer = Instantiate(circleBlue, targetPosition, Quaternion.identity) as GameObject;
            circleVisualizer.transform.SetParent(GameObject.FindGameObjectWithTag("SoundDirectionHolder").transform, false);
            Vector3 newSize = circleBlue.GetComponent<Transform>().localScale;
            //change size of the circle based on the  spectrumData
            newSize.y = amplitude * (float)0.05;
            newSize.x = amplitude * (float)0.05;
            circleBlue.GetComponent<Transform>().localScale = newSize;
        }
        else if (pitch ==0)
        {
            circleVisualizer = Instantiate(circleGreen, targetPosition, Quaternion.identity) as GameObject;
            circleVisualizer.transform.SetParent(GameObject.FindGameObjectWithTag("SoundDirectionHolder").transform, false);
            Vector3 newSize = circleGreen.GetComponent<Transform>().localScale;
            //change size of the circle based on the  spectrumData
            newSize.y = amplitude * (float)0.05;
            newSize.x = amplitude * (float)0.05;
            circleGreen.GetComponent<Transform>().localScale = newSize;
        }
    }
}