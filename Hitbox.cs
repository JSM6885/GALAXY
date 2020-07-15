using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
public class Hitbox : MonoBehaviour, IDamageable
{
    
    public float health = 50;
    public Slider hpSlider2;
	private Animator anim;
	private Drone drone;
	private NavMeshAgent nav;
	
	private float prevSpeed;


	void Start()
    {
		anim = GetComponent<Animator>();
		drone = GetComponent<Drone>();
		nav = GetComponent<NavMeshAgent>();
	}


    void Update()
    {

        hpSlider2.value = health;
        //health -= 10 * Time.deltaTime;

    }

        public void OnDamage(float damageAmount)
    {

		/*if (health<= 0)
        {
            //gameObject.SetActive(false);
            //Destroy(gameObject, 1.0f);
        }*/
		if ((health - damageAmount) <= 0 && drone.dea == false)
        {
			nav.enabled = false;
			health -= damageAmount; //체력이 0인 상태를 보여주기 위함
			//zombileaudio.clip = deathclip; //죽는 소리 재생
			//zombileaudio.Play();
			drone.dea = true;
            //animator.SetTrigger("death");
            //score.AddScore();
            //gameObject.SetActive(false);
            //Destroy(gameObject,1.5f);
        }

		else if(drone.dea == false)
		{
			anim.SetInteger("moving", 2);
			health -= damageAmount;
			
			//anim.SetInteger("moving", 5);
			prevSpeed = nav.speed;
			nav.speed = 0.1f;
			Invoke("test", 0.1f);
			Invoke("IncreseSpeed", 1f);
		}

    }

	private void test()
	{
		anim.SetInteger("moving", 7);
		
		
	}

	private void IncreseSpeed()
	{
		nav.speed = prevSpeed;
	}

}
