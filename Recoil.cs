using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    private float initAngle;
    public float recoilval;
    public float recoilval_re;
    public gun g;

    void Start()
    {
        initAngle = 0.0f;
    }

    void Update()
    {
        if (0.0f < initAngle)
        {
            initAngle = initAngle - recoilval_re;
            transform.Rotate(recoilval_re, 0, 0);
        }
    }
    public void RecoilAct()
    {
        if (g.shot == true)
        {
            initAngle += recoilval;
            transform.Rotate(-recoilval, 0, 0);
            g.shot = false;
        }
    }
}
