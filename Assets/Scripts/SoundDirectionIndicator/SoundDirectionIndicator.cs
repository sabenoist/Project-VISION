using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SoundDirectionIndicator : MonoBehaviour {
    [SerializeField]
    private float MaxTimer = 8.0f;
    private float timer = 8.0f;

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

    [SerializeField]
    private GameObject arrowIndicator;
    [SerializeField]
    private GameObject circleIndicator;

    private float amplitude;
    public float amplitudeScalar = 0.05f;
    private int pitch;
    private float button; //which button that has been pressed from server, signaling which graphic should be displayed

    public void Register(Transform target, Transform player, Action unRegister, float amplitude, int pitch, float button, float destroyTimer) 
    {
        this.amplitude = amplitude;
        this.pitch = pitch;
        this.Target = target;
        this.player = player;
        this.unRegister = unRegister;
        this.button = button;
        this.MaxTimer = destroyTimer;
        this.timer = destroyTimer;

        if (button == 1) 
        {
            VisualizeCircle(amplitude, pitch);
        } 
        else if (button == 2) 
        {
            VisualizeArrow(amplitude, pitch);
        }

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
        while (CanvasGroup.alpha < 1.0f) 
        {
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

    private void VisualizeArrow(float amplitude, int pitch)
    {
        // TODO: refactor this to avoid duplicating code with VisualizeCircle().
        GameObject circleVisualizer = Instantiate(arrowIndicator, transform.position, transform.rotation);
        circleVisualizer.transform.SetParent(GameObject.FindGameObjectWithTag("Pointer").transform, false);
        circleVisualizer.transform.position = GameObject.FindGameObjectWithTag("Pointer").transform.position;

        Vector3 newSize = circleVisualizer.GetComponent<Transform>().localScale;
        newSize.y = amplitude * amplitudeScalar;
        newSize.x = amplitude * amplitudeScalar;
        circleVisualizer.GetComponent<Transform>().localScale = newSize;

        if (pitch == 0) 
        {
            circleVisualizer.GetComponent<RawImage>().color = Color.green;
        } 
        else if (pitch == 1) 
        {
            circleVisualizer.GetComponent<RawImage>().color = Color.yellow;
        } 
        else if (pitch >= 2) 
        {
            circleVisualizer.GetComponent<RawImage>().color = Color.red;
        }
    }

    private void VisualizeCircle(float amplitude, int pitch) 
    {
        // TODO: refactor this to avoid duplicating code with VisualizeArrow().
        GameObject circleVisualizer = Instantiate(circleIndicator, transform.position, transform.rotation);
        circleVisualizer.transform.SetParent(GameObject.FindGameObjectWithTag("Pointer").transform, false);
        circleVisualizer.transform.position = GameObject.FindGameObjectWithTag("Pointer").transform.position;

        Vector3 newSize = circleVisualizer.GetComponent<Transform>().localScale;
        newSize.y = amplitude * amplitudeScalar;
        newSize.x = amplitude * amplitudeScalar;
        circleVisualizer.GetComponent<Transform>().localScale = newSize;

        if (pitch == 0) 
        {
            circleVisualizer.GetComponent<RawImage>().color = Color.green;
        } 
        else if (pitch == 1) 
        {
            circleVisualizer.GetComponent<RawImage>().color = Color.yellow;
        }
        else if (pitch >= 2) 
        {
            circleVisualizer.GetComponent<RawImage>().color = Color.red;
        }
    }
}