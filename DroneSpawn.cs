using UnityEngine;
using System.Collections;

//몬스터를 소환하는 스포너 스크립트

public class DroneSpawn : MonoBehaviour {
   // public GameObject zombie;//소환할 몬스터 
	public float MIN_TIME = 1;//소환 최소시간
	public float MAX_TIME = 3;//소환 최대시간
    public StageManage sm;//StageManage 스크립트
    public int monster_count;//소환할 몬스터 수량
    public int monster_num;//몬스터의 종류에 따른 번호
    private float Random_Pos;
    private Vector3 v;
    private Vector3 origin;

    void Start ()
    {
        sm = GameObject.Find("StageManager").GetComponent<StageManage>();//StageManage 스크립트
        v = transform.localPosition;
        origin = transform.localPosition;
    }

    private void Update()
    {
        
    }

    IEnumerator CreateDrone()//일정 시간마다 반복해서 정해진 몬스터를 소환
    {
        if (monster_count > 0)//소환할 몬스터의 수량이 남아있다면
        {
            while (monster_count > 0)//남아있는 동안 반복
            {
                Random_Pos = Random.Range(-15.0f, 15.0f);
                float createTime = Random.Range(MIN_TIME, MAX_TIME);//소환 최소 최대시간에서 랜덤하게
                v.x = Random_Pos;
                transform.position = v;
                yield return new WaitForSeconds(createTime);//위의 시간 동안 대기
                //Instantiate(zombie, transform.position, Quaternion.identity);//몬스터를 해당 위치에 소환
                if (monster_num == 0)
                {
                    sm.monster_obj01[monster_count - 1].transform.localPosition = this.transform.position;
                    if (sm.monster_obj01[monster_count - 1].GetComponent<Enemy>().Death==true)
                    {
                        sm.monster_obj01[monster_count - 1].GetComponent<Enemy>().HP = sm.monster_obj01[monster_count - 1].GetComponent<Enemy>().HP_origin;
                        sm.monster_obj01[monster_count - 1].SetActive(true);
                        sm.monster_obj01[monster_count - 1].GetComponent<Enemy>().Start();
                    }
                    else if(sm.monster_obj01[monster_count - 1].activeSelf == false)
                    {
                        sm.monster_obj01[monster_count - 1].SetActive(true);
                    }
                }
                else if (monster_num == 1)
                {
                    sm.monster_obj02[monster_count - 1].transform.localPosition = this.transform.position;
                    if (sm.monster_obj02[monster_count - 1].GetComponent<Enemy>().Death == true)
                    {
                        sm.monster_obj02[monster_count - 1].GetComponent<Enemy>().HP = sm.monster_obj02[monster_count - 1].GetComponent<Enemy>().HP_origin;
                        sm.monster_obj02[monster_count - 1].SetActive(true);
                        sm.monster_obj02[monster_count - 1].GetComponent<Enemy>().Start();
                    }
                    else if (sm.monster_obj02[monster_count - 1].activeSelf == false)
                    {
                        sm.monster_obj02[monster_count - 1].SetActive(true);
                    }
                }
                else if (monster_num == 2)
                {
                    sm.monster_obj03[monster_count - 1].transform.localPosition = this.transform.position;
                    if (sm.monster_obj03[monster_count - 1].GetComponent<FlyEnemy>().Death == true)
                    {
                        sm.monster_obj03[monster_count - 1].GetComponent<FlyEnemy>().HP = sm.monster_obj03[monster_count - 1].GetComponent<FlyEnemy>().HP_origin;
                        sm.monster_obj03[monster_count - 1].SetActive(true);
                        sm.monster_obj03[monster_count - 1].GetComponent<FlyEnemy>().Start();
                    }
                    else if (sm.monster_obj03[monster_count - 1].activeSelf == false)
                    {
                        sm.monster_obj03[monster_count - 1].SetActive(true);
                    }
                }
                else if (monster_num == 3)
                {
                    sm.monster_obj04[monster_count - 1].transform.localPosition = this.transform.position;
                    if (sm.monster_obj04[monster_count - 1].GetComponent<Enemy>().Death == true)
                    {
                        sm.monster_obj04[monster_count - 1].GetComponent<Enemy>().HP = sm.monster_obj04[monster_count - 1].GetComponent<Enemy>().HP_origin;
                        sm.monster_obj04[monster_count - 1].SetActive(true);
                        sm.monster_obj04[monster_count - 1].GetComponent<Enemy>().Start();
                    }
                    else if (sm.monster_obj04[monster_count - 1].activeSelf == false)
                    {
                        sm.monster_obj04[monster_count - 1].SetActive(true);
                    }
                }
                transform.position = origin;
                v = origin;
                monster_count--;//소환해야할 몬스터 수량 감소
            }
            //위의 반복문이 종료된 것은 소환할 몬스터가 더 이상 없다는 것
            sm.spawn_over[monster_num] = true;//해당 스포너의 스폰 종료를 TRUE로 변경
        }
    }

    public void StageChange()//스테이지가 변경되어 스테이지별 해당 몬스터의 수량을 불러오는 함수
    {
        if (monster_num == 0)//1번 몬스터일 경우
        {
            monster_count = sm.monster01[sm.stage];//소환할 몬스터 수량 갱신
        }
        else if (monster_num == 1)//2번 몬스터일 경우
        {
            monster_count = sm.monster02[sm.stage];//소환할 몬스터 수량 갱신
        }
        else if (monster_num == 2)//3번 몬스터일 경우
        {
            monster_count = sm.monster03[sm.stage];//소환할 몬스터 수량 갱신
        }
        else if (monster_num == 3)//4번 몬스터일 경우
        {
            monster_count = sm.monster04[sm.stage];//소환할 몬스터 수량 갱신
        }

        if (monster_count <= 0)//소환할 몬스터 수량이 없는 경우
        {
            sm.spawn_over[monster_num] = true;//바로 스포너 종료 TRUE
        }
        else if (monster_count > 0)//소환할 몬스터 수량이 있는 경우
        {
            StartCoroutine("CreateDrone");//소환 코루틴 실행
        }
    }
}
