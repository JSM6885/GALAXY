using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPLight : MonoBehaviour
{

    private Vector3 v;
    //private Vector3 origin;
    private float time;
    private bool on;
    public Manage m;
    // Start is called before the first frame update
    void Start()
    {
        m = GameObject.Find("Manager").GetComponent<Manage>();
        v = this.transform.localScale;
        // origin = this.transform.localScale;
        on = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (time >= 1.0f)
        {
            on = false;
        }
        else if (time <= 0)
        {
            on = true;
        }
        if (m.TimeStop == false)
        {
            if (on == true)
            {
                time += Time.deltaTime;
                v.y += 0.01f;
                v.z += 0.01f;
                v.x += 0.01f;
                this.transform.localScale = v;
            }
            else
            {
                time -= Time.deltaTime;
                v.y -= 0.01f;
                v.z -= 0.01f;
                v.x -= 0.01f;
                this.transform.localScale = v;
            }
        }


    }

    void LightSize(bool on)
    {
        if (on == true)
        {
            time += Time.deltaTime;
            v.y += 0.01f;
            v.z += 0.01f;
            v.x += 0.01f;
            this.transform.localScale = v;
        }
        else
        {
            time -= Time.deltaTime;
            v.y -= 0.01f;
            v.z -= 0.01f;
            v.x -= 0.01f;
            this.transform.localScale = v;
            // this.transform.localScale = origin;
        }
    }
}
