using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

//스테이지 관리 스크립트
//보스 관련도 여기에서 관리

public class StageManage : MonoBehaviour
{
    //스테이지 구분
    public int stage;//스테이지 변수
    public int stage_end;

    public bool[] spawn_over = new bool[] { };//각 스포너가 스폰을 종료했는지 판별

    //스테이지당 개체별 소환 수량
    public int[] monster01 = new int[] { };//1번 몬스터 스포너, 라운드 별 소환할 수를 결정
    public int[] monster02 = new int[] { };//2번 몬스터 스포너, 라운드 별 소환할 수를 결정
    public int[] monster03 = new int[] { };//3번 몬스터 스포너, 라운드 별 소환할 수를 결정
    public int[] monster04 = new int[] { };//4번 몬스터 스포너, 라운드 별 소환할 수를 결정

    public GameObject mob01_prefab;
    public GameObject mob02_prefab;
    public GameObject mob03_prefab;
    public GameObject mob04_prefab;

    [HideInInspector]
    public GameObject[] monster_obj01 = new GameObject[10];
    [HideInInspector]
    public GameObject[] monster_obj02 = new GameObject[10];
    [HideInInspector]
    public GameObject[] monster_obj03 = new GameObject[10];
    [HideInInspector]
    public GameObject[] monster_obj04 = new GameObject[10];

    public int[] monster_count = new int[] { };// 몬스터 다 잡았는지 카운트할 거 이걸로 스테이지가 넘어가게 바꿔야함

    //보스 관리
    public GameObject Boss;//보스 오브젝트
    public GameObject BossSlider;
    public Slider BossHP_Slider;
    public float Boss_HP;//보스 체력
    public float Boss_MAXHP;//보스 최대체력
    public float Speed;//보스 이동속도
    public int Pase_Change;//보스 페이즈 전환

    public DroneSpawn spawn0;//몹1 스포너
    public DroneSpawn spawn1;//몹2 스포너
    public DroneSpawn spawn2;//몹3 스포너
    public DroneSpawn spawn3;//몹4 스포너

    public int Kill_Count;//몹 죽인수
    public Text Wave;//웨이브 표시
    private int wave_count;
    //spublic Text Kill;//몹 킬 수 표시

    public bool WaveEnd;//웨이브 종료 판별
    public Text NextWaveTime;//다음 웨이브 시간 표시
    public float WaveTime;//다음 웨이브까지 시간
    private float WaveTimeOrigin;//웨이브 시간 초기화

    public GameObject WaveUI;//웨이브 타이머 UI
    private Vector3 v01;//웨이브 타이머 UI의 크기를 변경하기 위해
    public AudioSource WaveUI_Sound;
    public AudioClip CountDown;
    public AudioClip CountEnd;
    private float CountTime;

    private float EndTime;

    void Start()
    {
        WaveEnd = true;//웨이브 종료 판별 false 로 초기화
        stage = 0;//스테이지 초기화
        wave_count = 1;
        WaveTimeOrigin = WaveTime;//다음 웨이브까지의 시간 초기화를 위해 최초값 저장
        Wave.text = wave_count.ToString();//최초 스테이지 텍스트로 표시
        Kill_Count = 0;//킬 카운트 초기화
        for (int i = 0; i < stage_end; i++)//웨이브별 몹 수 초기화
        {
            monster_count[i] = monster01[i] + monster02[i] + monster03[i] + monster04[i];//몹1~4 웨이브별 수 총합
        }
        spawn_over = new bool[] { false, false, false, false };//각 스포너 스폰 종료 판별 초기화

        Speed = 1.0f;//보스 이동속도
        Pase_Change = 0;//페이즈 전환 변수 초기화

        spawn0 = GameObject.Find("Spawn").GetComponent<DroneSpawn>();//몹1 스포너
        spawn1 = GameObject.Find("Spawn (1)").GetComponent<DroneSpawn>();//몹2 스포너
        spawn2 = GameObject.Find("Spawn (2)").GetComponent<DroneSpawn>();//몹3 스포너
        spawn3 = GameObject.Find("Spawn (3)").GetComponent<DroneSpawn>();//몹4 스포너
        v01 = WaveUI.transform.localScale;//웨이브 타이머 UI 크기 변환하려고
        //WaveUIUPdate();
        BossHP_Slider.maxValue = Boss_MAXHP;
        BossHP_Slider.value = Boss_HP;
        EndTime = 0;

        CountTime = 0;
        CreateMonster();
        
    }
    void CreateMonster()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject obj1 = Instantiate(mob01_prefab);
            obj1.SetActive(false);
            monster_obj01[i] = obj1;            

            GameObject obj2 = Instantiate(mob02_prefab);
            obj2.SetActive(false);
            monster_obj02[i] = obj2;

            GameObject obj3 = Instantiate(mob03_prefab);
            obj3.SetActive(false);
            monster_obj03[i] = obj3;

            GameObject obj4 = Instantiate(mob04_prefab);
            obj4.SetActive(false);
            monster_obj04[i] = obj4;
        }
    }

    void Update()
    {
       
       // Kill.text = Kill_Count.ToString();//킬 카운트를 텍스트로 계속 표시해줌
        BossHP_Slider.value = Boss_HP;
        if (stage == stage_end)//스테이지10일 경우
        {
            BossSlider.SetActive(true);
            Instantiate<GameObject>(Boss);//보스 소환
            stage++;//스테이지 +
        }

        if (Boss_HP < (Boss_MAXHP / 2) && Pase_Change == 0)//보스의 체력이 절반 이상일 때
        {
            Pase_Change = 1;//페이즈 전환
        }
        else if (Boss_HP <= 0)
        {
            EndTime += Time.deltaTime;
            if (EndTime > 3)
            {
                SceneManager.LoadScene(5);
            }
        }
        if (stage <= (stage_end - 1) && monster_count[stage] <= 0)//스테이지가 9이하이고 해당 스테이지의 몹이 다 죽었을 때
        {
            WaveEnd = true;//웨이브 종료 TRUE
        }
        if (WaveEnd == true)//웨이브가 끝났을 때
        {
            WaveTime -= Time.deltaTime;//시간 감소
            NextWaveTime.text = Math.Round(WaveTime, 0).ToString();//소수점 자리는 빼고 텍스트에 표시

            if (WaveUI.transform.localScale.y <= 3)//웨이브 타이머 UI를 나타내는 연출
            {
                v01.y += 0.2f;//Y축 크기 증가
                WaveUI.transform.localScale = v01;//증가한 크기 적용
            }
            if (WaveTime <= 6)
            {
                CountTime += Time.deltaTime;
                if (CountTime >= 1)
                {
                    WaveUI_Sound.clip = CountDown;
                    WaveUI_Sound.Play();
                    CountTime = 0;
                }
            }

            if (WaveTime <= 0)//타이머가 다 됬을 때
            {
                wave_count++;
                WaveUI_Sound.clip = CountEnd;
                WaveUI_Sound.Play();
                Wave.text = wave_count.ToString();//스테이지(웨이브) UI 갱신
                StageOver_Check();//StageOver_Check() 함수 실행
                WaveTime = WaveTimeOrigin;//웨이브 타이머 초기화
                WaveEnd = false;//웨이브 종료 FALSE
            }
        }
        if (WaveEnd == false && WaveUI.transform.localScale.y > 0)//웨이브 타이머 UI 종료하는 연출
        {
            v01.y -= 1.0f;//Y축 크기 감소
            WaveUI.transform.localScale = v01;//감소된 크기 적용
            if (WaveUI.transform.localScale.y < 0)//너무 감소 됬을 경우
            {
                WaveUI.transform.localScale = new Vector3(3, 0, 3);//크기 값 고정
            }
        }
    }
    public void StageOver_Check()//스테이지(웨이브) 종료 체크 함수
    {
        if (monster_count[stage] <= 0)//해당 스테이지(웨이브)의 총 몬스터가 0 마리, 즉 전부 죽었을 때
        {
            if (spawn_over[0] == true && spawn_over[1] == true && spawn_over[2] == true && spawn_over[3] == true)//각 스포너가 전부 스폰을 종료했을 경우
            {
                stage++;//스테이지 변수 상승

                for (int i = 0; i < 4; i++)//각 스포너 종료 판정 초기화
                {
                    spawn_over[i] = false;
                }
                if (stage <= (stage_end - 1))//10 스테이지 까지만 작동하게 함, 11스테이지는 보스
                {
                    spawn0.StageChange();//스포너1 스테이지 변경 함수 작동
                    spawn1.StageChange();//스포너2 스테이지 변경 함수 작동
                    spawn2.StageChange();//스포너3 스테이지 변경 함수 작동
                    spawn3.StageChange();//스포너4 스테이지 변경 함수 작동
                }
            }
        }
        else if (stage == 0)
        {
            spawn0.StageChange();//스포너1 스테이지 변경 함수 작동
            spawn1.StageChange();//스포너2 스테이지 변경 함수 작동
            spawn2.StageChange();//스포너3 스테이지 변경 함수 작동
            spawn3.StageChange();//스포너4 스테이지 변경 함수 작동
        }
    }
    /*public void WaveUIUPdate()
    {
        WaveTime -= Time.deltaTime;//시간 감소
        NextWaveTime.text = Math.Round(WaveTime, 0).ToString();//소수점 자리는 빼고 텍스트에 표시
        if (WaveUI.transform.localScale.y <= 1)//웨이브 타이머 UI를 나타내는 연출
        {
            v01.y += 0.2f;//Y축 크기 증가
            WaveUI.transform.localScale = v01;//증가한 크기 적용
        }
        if (WaveTime <= 0)//타이머가 다 됬을 때
        {
            StageOver_Check();//StageOver_Check() 함수 실행
            WaveTime = WaveTimeOrigin;//웨이브 타이머 초기화
            WaveEnd = false;//웨이브 종료 FALSE
        }
    }*/
    
}
