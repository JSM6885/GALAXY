using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    private float time=0;
    public ParticleSystem effect;
    // Start is called before the first frame update
    void Start()
    {


        transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time > 2.0f)
        {
            effect.Stop();
            time = 0;
        }

    }
}
