using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCameraPosition : MonoBehaviour
{
    public Transform transform;
    public Text text;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        text.text = transform.rotation.x.ToString();
    }
}
