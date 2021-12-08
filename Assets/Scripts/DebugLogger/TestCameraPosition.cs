using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestCameraPosition : MonoBehaviour
{
    public Transform canvasTransform;
    public Text IpadX;
    public Text IpadY;
    public Text IpadZ;
    public Text CanvasX;
    public Text CanvasY;
    public Text CanvasZ;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CanvasX.text = canvasTransform.rotation.x.ToString();
        CanvasY.text = canvasTransform.rotation.y.ToString();
        CanvasZ.text = canvasTransform.rotation.z.ToString();

        IpadX.text = this.transform.rotation.x.ToString();
        IpadY.text = this.transform.rotation.y.ToString();
        IpadZ.text = this.transform.rotation.z.ToString();
    }
}