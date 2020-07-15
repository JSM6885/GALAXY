using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyUI : MonoBehaviour
{
    private float time;
    private float EndTime;
    private Vector3 v;
    private float money;
    public Text money_text;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.parent.name == "thc3(Clone)")
        {
            money = gameObject.transform.parent.GetComponent<FlyEnemy>().money;
        }
        else if (transform.parent.name == "hor_mon_1.1(Clone)")
        {
            money = gameObject.transform.parent.GetComponent<BossEnemy>().money;
        }
        else
        {
            money = gameObject.transform.parent.GetComponent<Enemy>().money;
        }

        if (transform.parent.name == "hor_mon_1.1(Clone)")
        {
            v.x = transform.parent.position.x;
            v.y = transform.parent.position.y + 9;
            v.z = transform.parent.position.z;
        }
        else if (transform.parent.name == "thc4 (1)(Clone)")
        {
            v.x = transform.parent.position.x;
            v.y = transform.parent.position.y + 2;
            v.z = transform.parent.position.z;
        }
        else
        {
            v = transform.parent.localPosition;
        }
        transform.parent = null;
        time = 0;
        EndTime = 1.5f;

    }

    // Update is called once per frame
    void Update()
    {
        if (time < EndTime)
        {
            time += Time.deltaTime;
            v.y += 0.03f;
            this.transform.localPosition = v;
            money_text.text = money.ToString();
        }
        else
        {
            Destroy(this.gameObject);
        }

    }
}
