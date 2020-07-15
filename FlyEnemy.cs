using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//비행 몬스터의 스크립트

public class FlyEnemy : MonoBehaviour, IDamageable
{
    public static Enemy Instance;
    Transform tower;//목표물
    public float ATTACK_TIME;//공격 시간
    private float attackTime = 0;//공격 시간 경과 변수
    public AudioSource zombileaudio;//소리
    public AudioClip deathclip;//죽는 소리
    public AudioClip attackclip;//공격 소리
    public AudioClip hitclip;//피격 소리
    public Animator anim;//애니메이션
    public float ATTACK_DISTANCE;//공격 사정거리
    public float damage;//데미지
    public float HP;//체력
    public float HP_origin;
    public float speed;//이동속도
    public bool Death;//사망 판정
    public int money;//주는 재화량
    Manage m;//Manage 스크립트
    StageManage sm;//StageManage 스크립트
    public ParticleSystem HitEffect;//피격 이펙트
    public ParticleSystem AttackEffect;//공격 이펙트
    private float speed_temp;//피격 시 경직 후 다시 원래 속도로 돌리기 위한 변수
    private float delayTime;//피격 경직 시간 계산
    private float HP_temp;//피격 시 체력에 변동이 있다는 것을 비교
   // public GameObject DmgText;
    public GameObject MoneyText;
    private int Random_Target;
    public Slider HP_Slide;
    private float DeathTime;
    public void Start()
    {
        speed = 0.15f;
        Random_Target = Random.Range(0, 3);
        if (Random_Target == 0)
        {
            tower = GameObject.Find("Tower").transform; //길찾기 목표 오브젝트
        }
        else if (Random_Target == 1)
        {
            tower = GameObject.Find("Tower (1)").transform; //길찾기 목표 오브젝트
        }
        else if (Random_Target == 2)
        {
            tower = GameObject.Find("Tower (2)").transform; //길찾기 목표 오브젝트
        }
        m = GameObject.Find("Manager").GetComponent<Manage>();//Manage 스크립트
        sm = GameObject.Find("StageManager").GetComponent<StageManage>();//StageManage 스크립트
        attackTime = ATTACK_TIME;//공격시간 초기화
        anim = GetComponent<Animator>();//애니메이션
        anim.SetBool("move", true);//이동 애니메이션 시작
        Death = false;//사망 판정 FALSE
        speed_temp = speed;//피격 대비 속도값 저장
        HP_temp = HP;//피격판정을 위한 체력 임시값 저장
        delayTime = 0;//피격 경직시간 초기화
        HP_Slide.maxValue = HP;
        HP_Slide.value = HP;
        HP_origin = HP;
        DeathTime = 0;
    }
    
    void Update()
    {
        if (HP>0 && HP_temp != HP)//피격 시 경직 스크립트
        {
            speed = 0;//이동 정지를 위한 속도 0
            delayTime += Time.deltaTime;//경직 시간 계산
            if (delayTime >= 0.5f)//경직 해제 
            {
                speed = speed_temp;//속도 원상복귀
                HP_temp = HP;//경직 비교를 위한 체력 재입력
                delayTime = 0;//경직시간 초기화
            }
        }
        if (anim.GetBool("move") == true)//이동 애니메이션이 재생 중일 경우
        {
            transform.position = new Vector3(transform.position.x, 1.5f, transform.position.z);//오브젝트의 높이 고정
            this.transform.position = Vector3.Lerp(transform.position, tower.transform.position, speed * Time.deltaTime);//오브젝트 이동속도만큼 이동
        }
        if (HP > 0)//살아있을 때
        {
            if (Vector3.Distance(transform.position,tower.position) <= ATTACK_DISTANCE) //좀비 공격 범위 안에 들어오면,
            {
                speed = 0;//이동 정지
                anim.SetBool("move", false);//이동 애니메이션 정지
                attackTime += Time.deltaTime; //경과 시간을 체크해서 좀비 공격시간 간격될 때마다 공격
                if (attackTime > ATTACK_TIME)//공격시간이 됬을 때
                {
                    zombileaudio.clip = attackclip;//공격 소리
                    zombileaudio.Play();//해당 소리 재생
                    anim.SetTrigger("Attack");//공격 애니메이션 재생
                    AttackEffect.Play();//공격 이펙트 재생
                    Tower.Instance.Damage(damage);//목표의 데미지 함수 실행
                    attackTime = 0;//공격시간 초기화
                }
            }
        }
        if (gameObject.activeSelf == true && HP <= 0)
        {
            DeathTime += Time.deltaTime;
            if (DeathTime > 2.0f)
            {
                Death = true;//사망 판정 TRUE
                gameObject.SetActive(false);
            }
        }

    }
    public void OnDamage(float damageAmount)//받는 데미지 함수
    {
        HP -= damageAmount;//체력에서 받은 데미지만큼 감소
        HP_Slide.value = HP;
        if (Death == false)
        {
            //GameObject dmg = Instantiate<GameObject>(DmgText, this.transform.position, this.transform.rotation) as GameObject;
            //dmg.transform.parent = this.transform;
        }
        if (HP > 0)//살아있을 때
        {
            HitEffect.Play();//피격 이펙트 재생
            zombileaudio.clip = hitclip;//피격 소리 
            zombileaudio.Play();//해당 소리 재생
            anim.SetTrigger("Hit");//피격 애니메이션 재생
        }
        else
        {
            if (Death == false)
            {
                if (DeathTime == 0)
                {
                    GameObject money_t = Instantiate<GameObject>(MoneyText, this.transform.position, this.transform.rotation) as GameObject;
                    money_t.transform.parent = this.transform;
                    m.money += money;//재화 추가
                    anim.SetTrigger("Death");//죽는 애니메이션 재생agent.speed = 0;//이동속도 0으로 정지
                    m.moneyChange();//재화 UI 갱신
                    zombileaudio.clip = deathclip;//죽는 소리
                    zombileaudio.Play();//해당 소리 재생
                    sm.monster_count[sm.stage]--;//해당 스테이지(웨이브) 의 총 몹의 수에서 감소
                    sm.Kill_Count++;//죽인 숫자 추가      
                    DeathTime += Time.deltaTime;
                }

                //Destroy(gameObject, 2.0f);//오브젝트 파괴
            }
        }
    }
}
