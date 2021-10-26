using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public GameObject[] CircleGroup; //store the instantiated prefab

    public float[] volumnData;//to be received from the server
    public string[] pitch; //array of pitch to be received from servcer
    public Vector3[] position;////to be received from the server

    float appearing_speed = 2f;

    public void Register(Transform target, Transform player, Action unRegister)
    {
        this.Target = target;
        this.player = player;
        this.unRegister = unRegister;

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

    // Update is called once per frame
    void FixedUpdate()
    {

        for (int i = 0; i < volumnData.Length; i++)
        {

            if (pitch[i] == "High" && !CircleGroup[i])
            {
                //instantiate circle prefab and specify position with Vector3
                CircleGroup[i] = Instantiate(circleRed, position[i], Quaternion.identity) as GameObject;
                //instantiate prefab as child of CircleCanvas
                CircleGroup[i].transform.SetParent(GameObject.FindGameObjectWithTag("CircleCanvas").transform, false);

                Vector3 newSize = circleRed.GetComponent<Transform>().localScale;
                //change size of the circle based on the  spectrumData
                newSize.y = volumnData[i];
                newSize.x = volumnData[i];
                circleRed.GetComponent<Transform>().localScale = newSize;

                //slowly fade out the circle
                StartCoroutine(FadeOutMaterial(1.5f, CircleGroup[i]));
                Destroy(CircleGroup[i], 1.5f);//destory circle prefab after 1.5 seconds

                //remove used spectrumData from array to prevent it instantiate prefab again
                StartCoroutine(ShowAndHide(0.6f));
            }

            //pitch represent color and volumn represent size
            else if (pitch[i] == "Medium" && !CircleGroup[i])
            {
                //instantiate circle prefab and specify position with Vector3
                //receive position value from server
                CircleGroup[i] = Instantiate(circleBlue, position[i], Quaternion.identity) as GameObject;
                //instantiate prefab as child of CircleCanvas
                CircleGroup[i].transform.SetParent(GameObject.FindGameObjectWithTag("CircleCanvas").transform, false);

                Vector3 newSize = circleBlue.GetComponent<Transform>().localScale;
                //change size of the circle based on the  spectrumData
                newSize.y = volumnData[i];
                newSize.x = volumnData[i];
                circleBlue.GetComponent<Transform>().localScale = newSize;
                StartCoroutine(FadeOutMaterial(1.5f, CircleGroup[i]));
                Destroy(CircleGroup[i], 1.5f);//destory circle prefab  after 1.5 seconds

                //remove used spectrumData from array to prevent it instantiate prefab again
                StartCoroutine(ShowAndHide(0.6f));
            }

            else if (pitch[i] == "Low" && !CircleGroup[i])
            {
                //instantiate circle prefab and specify position with Vector3
                CircleGroup[i] = Instantiate(circleGreen, position[i], Quaternion.identity) as GameObject;
                //instantiate prefab as child of CircleCanvas
                CircleGroup[i].transform.SetParent(GameObject.FindGameObjectWithTag("CircleCanvas").transform, false);

                Vector3 newSize = circleGreen.GetComponent<Transform>().localScale;
                //change size of the circle based on the  spectrumData
                newSize.y = volumnData[i];
                newSize.x = volumnData[i];
                circleGreen.GetComponent<Transform>().localScale = newSize;
                StartCoroutine(FadeOutMaterial(1.5f, CircleGroup[i]));
                Destroy(CircleGroup[i], 1.5f);//destory circle prefab after 1.5 seconds

                //remove used spectrumData from array to prevent it instantiate prefab again
                StartCoroutine(ShowAndHide(0.6f));

            }


        }
    }

    IEnumerator ShowAndHide(float delay)
    {
        for (int i = 0; i < volumnData.Length; i++)
        {
            yield return new WaitForSeconds(delay);
            volumnData = volumnData.Skip(i).ToArray();
        }


    }

    //slowly fade out the circle
    IEnumerator FadeOutMaterial(float fadeSpeed, GameObject circle)
    {
        RawImage image = circle.transform.GetComponent<RawImage>();
        Color oldColor = image.color;
        float alphaValue = image.color.a;

        while (image.color.a > 0f)
        {
            alphaValue -= Time.deltaTime / fadeSpeed;
            image.color = new Color(oldColor.r, oldColor.g, oldColor.b, alphaValue);
            yield return null;
        }
        image.color = new Color(oldColor.r, oldColor.g, oldColor.b, 0f);

    }
}