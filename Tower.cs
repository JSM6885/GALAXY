using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Tower : MonoBehaviour {

	public static Tower Instance;
    
	public float hp = 0;
    public AudioSource toweraudio;
    public AudioClip hitclip;

    public Manage m;

    bool gameOver = false; //시작할 때는 게임오버가 아니다.

	void Awake()
	{
        
		if(Instance == null)
			Instance = this;
	}

	void Start()
	{
        m = GameObject.Find("Manager").GetComponent<Manage>();
    }

    private void Update() //만약 gameOver 가 true이면 게임 재시작한다
    {
        hp = m.HeartPoint;
    }

   

    public void Damage(float d)
    {
        if (hp > 0)
        {
            hp -= d; //데미지 입으면 HP -1 씩 감소
            toweraudio.clip = hitclip; //맞는 소리 재생
            toweraudio.Play();
            m.HeartPoint = hp;
        }        
	}
}
