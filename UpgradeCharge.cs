using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCharge : MonoBehaviour
{
    public GameObject[] Charge;
    public GunMenu gm;
    public int gunNUM;
    public int UpgradeNUM;
    
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GunMenu").GetComponent<GunMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.chargeup[gunNUM, UpgradeNUM] == 1)
        {
            Charge[0].SetActive(true);
        }
        else if (gm.chargeup[gunNUM, UpgradeNUM] == 2)
        {
            Charge[1].SetActive(true);
        }
        else if (gm.chargeup[gunNUM, UpgradeNUM] == 3)
        {
            Charge[2].SetActive(true);
        }
        else if (gm.chargeup[gunNUM, UpgradeNUM] == 4)
        {
            Charge[3].SetActive(true);
        }
        else if (gm.chargeup[gunNUM, UpgradeNUM] == 5)
        {
            Charge[4].SetActive(true);
        }
    }
}
