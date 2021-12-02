using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioBarDecay : MonoBehaviour
{
    public float decayScalar = 0.25f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 scale = transform.localScale;
        if (scale.y > 0)
        {
            Vector3 newScale = new Vector3(scale.x, scale.y - decayScalar, scale.z);

            if (newScale.y < 0)
            {
                newScale = new Vector3(scale.x, 0, scale.z);
            }

            transform.localScale = newScale;
        }
    }
}
