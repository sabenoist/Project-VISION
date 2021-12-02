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
    [SerializeField]
    private GameObject circleArrowIndicator;
    [SerializeField]
    private GameObject circleArrow2Indicator;
    [SerializeField]
    private GameObject adjustableArrow1;
    [SerializeField]
    private GameObject adjustableArrow2;
    [SerializeField]
    private GameObject adjustableArrow3;
    [SerializeField]
    private GameObject soundBars;

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
        else if (button == 3)
        {
            VisualizeCircleArrow(amplitude, pitch);
        }
        else if (button == 4)
        {
            VisualizeCircleArrow2(amplitude, pitch);
        }
        else if (button == 5)
        {
            VisualizeAdjustableArrow(amplitude, pitch);
        }
        else if (button == 6)
        {
            VisualizeSoundBars(pitch);
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
    private void VisualizeCircleArrow(float amplitude, int pitch)
    {
        // TODO: refactor this to avoid duplicating code with VisualizeArrow().
        GameObject circleArrowVisualizer = Instantiate(circleArrowIndicator, transform.position, transform.rotation);
        circleArrowVisualizer.transform.SetParent(GameObject.FindGameObjectWithTag("Pointer").transform, false);
        circleArrowVisualizer.transform.position = GameObject.FindGameObjectWithTag("Pointer").transform.position;
        GameObject arrow = circleArrowVisualizer.transform.GetChild(0).gameObject;

        Vector3 newSize = arrow.GetComponent<Transform>().localScale;
        newSize.y = amplitude * 0.02f;
        //  newSize.x = amplitude * amplitudeScalar;
        arrow.GetComponent<Transform>().localScale = newSize;

        if (pitch == 0)
        {
            arrow.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = Color.green;
            circleArrowVisualizer.GetComponent<RawImage>().color = Color.green;
        }
        else if (pitch == 1)
        {
            //arrow.GetComponent<RawImage>().color = Color.yellow;
            arrow.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = Color.blue;
            circleArrowVisualizer.GetComponent<RawImage>().color = Color.blue;
        }
        else if (pitch >= 2)
        {
            arrow.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = Color.red;
            circleArrowVisualizer.GetComponent<RawImage>().color = Color.red;
        }
    }
    private void VisualizeCircleArrow2(float amplitude, int pitch)
    {
        // TODO: refactor this to avoid duplicating code with VisualizeArrow().
        GameObject circleArrowVisualizer = Instantiate(circleArrow2Indicator, transform.position, transform.rotation);
        circleArrowVisualizer.transform.SetParent(GameObject.FindGameObjectWithTag("Pointer").transform, false);
        circleArrowVisualizer.transform.position = GameObject.FindGameObjectWithTag("Pointer").transform.position;

        Vector3 newSize = circleArrowVisualizer.GetComponent<Transform>().localScale;
        newSize.y = amplitude * amplitudeScalar;
        newSize.x = amplitude * amplitudeScalar;
        circleArrowVisualizer.GetComponent<Transform>().localScale = newSize;

        if (pitch == 0)
        {
            circleArrowVisualizer.GetComponent<RawImage>().color = Color.green;
        }
        else if (pitch == 1)
        {
            circleArrowVisualizer.GetComponent<RawImage>().color = Color.yellow;
        }
        else if (pitch >= 2)
        {
            circleArrowVisualizer.GetComponent<RawImage>().color = Color.red;
        }
    }
    private void VisualizeAdjustableArrow(float amplitude, int pitch)
    {
        // TODO: refactor this to avoid duplicating code with VisualizeArrow().
        if (amplitude >= 70 && amplitude <= 100)
        {
            GameObject circleArrowVisualizer = Instantiate(adjustableArrow1, transform.position, transform.rotation);
            circleArrowVisualizer.transform.SetParent(GameObject.FindGameObjectWithTag("Pointer").transform, false);
            circleArrowVisualizer.transform.position = GameObject.FindGameObjectWithTag("Pointer").transform.position;

            Vector3 newSize = circleArrowVisualizer.GetComponent<Transform>().localScale;
            newSize.y = amplitude * amplitudeScalar;
            newSize.x = amplitude * amplitudeScalar;
            circleArrowVisualizer.GetComponent<Transform>().localScale = newSize;

            if (pitch == 0)
            {
                circleArrowVisualizer.GetComponent<RawImage>().color = Color.green;
            }
            else if (pitch == 1)
            {
                circleArrowVisualizer.GetComponent<RawImage>().color = Color.yellow;
            }
            else if (pitch >= 2)
            {
                circleArrowVisualizer.GetComponent<RawImage>().color = Color.red;
            }
        }

        if (amplitude > 100 && amplitude <= 130)
        {
            GameObject circleArrowVisualizer = Instantiate(adjustableArrow2, transform.position, transform.rotation);
            circleArrowVisualizer.transform.SetParent(GameObject.FindGameObjectWithTag("Pointer").transform, false);
            circleArrowVisualizer.transform.position = GameObject.FindGameObjectWithTag("Pointer").transform.position;

            Vector3 newSize = circleArrowVisualizer.GetComponent<Transform>().localScale;
            newSize.y = amplitude * amplitudeScalar;
            newSize.x = amplitude * amplitudeScalar;
            circleArrowVisualizer.GetComponent<Transform>().localScale = newSize;

            if (pitch == 0)
            {
                circleArrowVisualizer.GetComponent<RawImage>().color = Color.green;
                circleArrowVisualizer.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = Color.green;
            }
            else if (pitch == 1)
            {
                circleArrowVisualizer.GetComponent<RawImage>().color = Color.yellow;
                circleArrowVisualizer.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = Color.green;
            }
            else if (pitch >= 2)
            {
                circleArrowVisualizer.GetComponent<RawImage>().color = Color.red;
                circleArrowVisualizer.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = Color.yellow;
            }
        }

        if (amplitude > 130 && amplitude <= 160)
        {
            GameObject circleArrowVisualizer = Instantiate(adjustableArrow3, transform.position, transform.rotation);
            circleArrowVisualizer.transform.SetParent(GameObject.FindGameObjectWithTag("Pointer").transform, false);
            circleArrowVisualizer.transform.position = GameObject.FindGameObjectWithTag("Pointer").transform.position;

            Vector3 newSize = circleArrowVisualizer.GetComponent<Transform>().localScale;
            newSize.y = amplitude * amplitudeScalar;
            newSize.x = amplitude * amplitudeScalar;
            circleArrowVisualizer.GetComponent<Transform>().localScale = newSize;

            if (pitch == 0)
            {
                circleArrowVisualizer.GetComponent<RawImage>().color = Color.green;
                circleArrowVisualizer.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = Color.green;
                circleArrowVisualizer.transform.GetChild(1).gameObject.GetComponent<RawImage>().color = Color.green;
            }
            else if (pitch == 1)
            {
                circleArrowVisualizer.GetComponent<RawImage>().color = Color.yellow;
                circleArrowVisualizer.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = Color.yellow;
                circleArrowVisualizer.transform.GetChild(1).gameObject.GetComponent<RawImage>().color = Color.green;
            }
            else if (pitch >= 2)
            {
                circleArrowVisualizer.GetComponent<RawImage>().color = Color.red;
                circleArrowVisualizer.transform.GetChild(0).gameObject.GetComponent<RawImage>().color = Color.yellow;
                circleArrowVisualizer.transform.GetChild(1).gameObject.GetComponent<RawImage>().color = Color.green;
            }
        }
    }

    private void VisualizeSoundBars(int pitch)
    {
        GameObject bars = Instantiate(soundBars, transform.position, transform.rotation);
        bars.transform.SetParent(GameObject.FindGameObjectWithTag("Pointer").transform, false);
        bars.transform.position = GameObject.FindGameObjectWithTag("Pointer").transform.position;

        if (pitch == 0)
        {
            foreach (GameObject bar in bars.GetComponent<AudioPeer>().bars)
            {
                bar.GetComponent<Image>().color = Color.green;
            }
        }
        if (pitch == 1)
        {
            foreach (GameObject bar in bars.GetComponent<AudioPeer>().bars)
            {
                bar.GetComponent<Image>().color = Color.yellow;
            }
        }
        if (pitch >= 2)
        {
            foreach (GameObject bar in bars.GetComponent<AudioPeer>().bars)
            {
                bar.GetComponent<Image>().color = Color.red;
            }
        }
    }
}