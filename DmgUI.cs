using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DmgUI : MonoBehaviour
{
    private float time;
    private float EndTime;
    private Vector3 v;
    public GunManage gm;
    private float dmg;
    public Text dmg_text;
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GunManager").GetComponent<GunManage>();   //GunManage 스크립트     
        if(transform.parent.name== "hor_mon_1.1(Clone)")
        {
            v.x = transform.parent.position.x;
            v.y = transform.parent.position.y + 10;
            v.z = transform.parent.position.z+10;
        }
        else if (transform.parent.name == "thc4 (1)(Clone)")
        {
            v.x = transform.parent.position.x;
            v.y = transform.parent.position.y + 4;
            v.z = transform.parent.position.z;
        }
        else
        {
            v = transform.parent.localPosition;
        }
        transform.parent = null;
        time = 0;        
        EndTime = 1.5f;
        dmg = gm.Current_Gun_Damage;
    }

    // Update is called once per frame
    void Update()
    {
        if(time < EndTime)
        {
            time += Time.deltaTime;
            v.y += 0.02f;
            this.transform.localPosition = v;
            dmg_text.text = dmg.ToString();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
