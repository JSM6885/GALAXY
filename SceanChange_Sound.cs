using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceanChange_Sound : MonoBehaviour
{
    private float time;
    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if(time > 3.0f)
        {
            Destroy(this);
        }
    }
}
