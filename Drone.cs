using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]


//옛날 몬스터 스크립트
//현재는 사용하지 않음ㄴ

public class Drone : MonoBehaviour {
    public static Drone Instance;
    UnityEngine.AI.NavMeshAgent agent;
	Transform tower;
    [HideInInspector]    public float ATTACK_TIME = 2.5f; //좀비의 공격 시간 간격
	float attackTime = 0; //시간 경과를 체크하는 변수
    public float hp;
    Hitbox hitbox;
    [System.NonSerialized]
    public bool dea = false;
    public AudioSource zombileaudio;
    public AudioClip deathclip;
    public AudioClip attackclip;
    Tower tower_hp;

	private Animator anim;

    public float damage;

	

	public float ATTACK_DISTANCE = 2.0f; //좀비의 공격 가능거리
	// Use this for initialization

	void Start () {
		agent = GetComponent<UnityEngine.AI.NavMeshAgent>(); //길찾기 AI네비게이션
		tower = GameObject.Find("Tower").transform; //길찾기 목표 오브젝트
		agent.destination = tower.position;
        hitbox = GetComponent<Hitbox>();
        attackTime = ATTACK_TIME;
        tower_hp = GameObject.Find("Tower").GetComponent<Tower>();

		anim = GetComponent<Animator>();
		anim.SetInteger("moving", 0);
	}


	void Update()
	{
        hp = hitbox.health;
        if (dea == false)
        {
            if (agent.remainingDistance <= ATTACK_DISTANCE) //좀비 공격 범위 안에 들어오면,
            {

                attackTime += Time.deltaTime; //경과 시간을 체크해서 좀비 공격시간 간격될 때마다 공격
                anim.SetInteger("moving", 5);
                if (attackTime > ATTACK_TIME)
                {
                    anim.SetInteger("moving", 1);
                    attackTime = 0;
                    Tower.Instance.Damage(damage);// 타워(목표물)의 데미지 함수 실행
                }
            }
        }
    }

   
}
