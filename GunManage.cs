using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunManage : MonoBehaviour
{
   /* public int m_MaxAmmo = 13; //탄창의 최대 탄약 수
    public float m_TimeBetFire = 0.3f; //발사 사이 시간간격
    public float m_Damage = 25;//총이 주는 데미지
    public float m_ReloadTime = 2.0f;
    public float m_FireDistance = 100f; //총의 사정거리*/

    public int[] m_MaxAmmo;
    public float[] m_TimeBetFire;
    public float[] m_Damage;
    public float[] m_ReloadTime;
    public float[] m_FireDistance;

    public int Current_Gun_MaxAmmo;
    public float Current_Gun_TimeBetFire;
    public float Current_Gun_Damage;
    public float Current_Gun_ReloadTime;
    public float Current_Gun_FireDistance;

    public gunselect gs;


    // Start is called before the first frame update
    void Start()
    {
        gs= GameObject.Find("RightHand").GetComponent<gunselect>();

        Current_Gun_MaxAmmo=0;
        Current_Gun_TimeBetFire = 0.0f;
        Current_Gun_Damage = 0;
        Current_Gun_ReloadTime = 0;
        Current_Gun_FireDistance = 0;

        m_MaxAmmo[0] = 20;
        m_TimeBetFire[0] = 0.3f;
        m_Damage[0] = 10;
        m_ReloadTime[0] = 2.0f;
        m_FireDistance[0] = 100;

        m_MaxAmmo[1] = 10;
        m_TimeBetFire[1] = 2.0f;
        m_Damage[1] = 100.0f;
        m_ReloadTime[1] = 2.5f;
        m_FireDistance[1] =200.0f;

        m_MaxAmmo[2] = 100000;
        m_TimeBetFire[2] = 0.1f;
        m_Damage[2] = 50.0f;
        m_ReloadTime[2] = 2.0f;
        m_FireDistance[2] = 100.0f;

        Debug.Log("GunManage.Start");

    }

    public void GunInit()
    {
        //Debug.Log("GunManage.GunInit gs.gun_temp = "+ gs.gun_temp);
        switch (gs.gun_temp)
        {
            case 1:
                Current_Gun_MaxAmmo = m_MaxAmmo[0];
                Current_Gun_TimeBetFire = m_TimeBetFire[0];
                Current_Gun_Damage = m_Damage[0];
                Current_Gun_ReloadTime = m_ReloadTime[0];
                Current_Gun_FireDistance = m_FireDistance[0];
                break;
            case 2:
                Current_Gun_MaxAmmo = m_MaxAmmo[1];
                Current_Gun_TimeBetFire = m_TimeBetFire[1];
                Current_Gun_Damage = m_Damage[1];
                Current_Gun_ReloadTime = m_ReloadTime[1];
                Current_Gun_FireDistance = m_FireDistance[1];
                break;
            case 3:
                Current_Gun_MaxAmmo = m_MaxAmmo[2];
                Current_Gun_TimeBetFire = m_TimeBetFire[2];
                Current_Gun_Damage = m_Damage[2];
                Current_Gun_ReloadTime = m_ReloadTime[2];
                Current_Gun_FireDistance = m_FireDistance[2];
                break;
        }
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    switch (gs.gun_temp)
    //    {
    //        case 1:
    //            Current_Gun_MaxAmmo = m_MaxAmmo[0];
    //            Current_Gun_TimeBetFire = m_TimeBetFire[0];
    //            Current_Gun_Damage = m_Damage[0];
    //            Current_Gun_ReloadTime = m_ReloadTime[0];
    //            Current_Gun_FireDistance = m_FireDistance[0];
    //            break;
    //        case 2:
    //            Current_Gun_MaxAmmo = m_MaxAmmo[1];
    //            Current_Gun_TimeBetFire = m_TimeBetFire[1];
    //            Current_Gun_Damage = m_Damage[1];
    //            Current_Gun_ReloadTime = m_ReloadTime[1];
    //            Current_Gun_FireDistance = m_FireDistance[1];
    //            break;
    //        case 3:
    //            Current_Gun_MaxAmmo = m_MaxAmmo[2];
    //            Current_Gun_TimeBetFire = m_TimeBetFire[2];
    //            Current_Gun_Damage = m_Damage[2];
    //            Current_Gun_ReloadTime = m_ReloadTime[2];
    //            Current_Gun_FireDistance = m_FireDistance[2];
    //            break;
    //    }
    //}
}
