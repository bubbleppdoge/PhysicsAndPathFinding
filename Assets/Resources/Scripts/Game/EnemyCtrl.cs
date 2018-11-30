using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyCtrl : MonoBehaviour {

	public Transform player;			//玩家Transform
	public GameObject bloodPre;			//鲜血特效预制体
	private NavMeshAgent agent;			//AI代理
	private Animator anim;				//zombie动画
	private PlayerCtrl pCtrl;			//玩家控制脚本
	private GameCtrl gCtrl;				//游戏控制脚本

	public float backDis = 5f;			//后退距离  PS：AI被攻击后，后退超过这个距离，就会停止（并非必要，但是控制准确，可通过改变后退时间控制距离）
	public float backTime = 2f;			//后退的总时间（后退2s，会是很长的一段距离）
	private float curBackTime;			//当前后退时间

	public int damage = 10;				//攻击伤害
	public float waitForAttack = 1f;	//攻击间隔
	private float curWaitTime;			//间隔计时

	public float timeToChangeSpeed = 5f;//速度改变间隔
	public float onceSpeedValue = 0.3f;	//一次速度改变的大小

	private bool beAttacked;			//被攻击状态


	void Start()
	{
		curWaitTime = 0;
		curBackTime = 0;
		beAttacked = false;

		agent = GetComponent<NavMeshAgent> ();
		anim = GetComponent<Animator> ();
		pCtrl = player.gameObject.GetComponent<PlayerCtrl> ();
		gCtrl = GameObject.Find ("GameCtrl").GetComponent<GameCtrl> ();
	}

	void Update () {
		if (gCtrl.gameOver)		//游戏结束后停止执行
			return;
		
		agent.speed = 6 - (int)(gCtrl.gameTime / timeToChangeSpeed) * onceSpeedValue;			//速度随时间改变

		if(!beAttacked)
			agent.SetDestination (player.position);												//攻击目标（玩家）
		
		Attack ();																				//攻击

		BeAttacked ();																			//被攻击
	}

	void Attack()
	{
		if (Vector3.Distance (transform.position, player.position) > agent.stoppingDistance) {	//小于攻击距离则走向目标
			if (!beAttacked)
				anim.SetFloat ("Speed", agent.speed);											//播放行走动画
			curWaitTime = waitForAttack / 2;													//在走向目标过程中，攻击cd保持一半，加快接近目标后第一次攻击速度
		} else if (curWaitTime >= waitForAttack) {
			curWaitTime = 0;																	//攻击后重置攻击间隔								
			anim.SetTrigger ("Attack");															//播放攻击动画
			pCtrl.TakeDamage(damage);															//玩家受伤
			pCtrl.beAttacked = true;
		} else {
			transform.LookAt (new Vector3(player.position.x, transform.position.y, player.position.z));		//攻击时面向目标
			curWaitTime += Time.deltaTime;														//攻击间隔计时
			anim.SetFloat ("Speed", 0);															//播放站立动画
		}
	}

	void BeAttacked()
	{
		anim.SetBool ("BeAttacked", beAttacked);
		if (beAttacked) {
			curBackTime += Time.deltaTime;														//后退时间计时
			transform.position = Vector3.Lerp(transform.position, transform.position - transform.forward, curBackTime /backTime);		//后退
			if (Vector3.Distance (transform.position, player.position) >= backDis - 0.02f) {	//后退到指定距离后停止
				beAttacked = false;
				gCtrl.score += 1;																//被攻击后玩家分数+1
				curBackTime = 0;																//重置后退计时
			}
		}
	}

	void OnTriggerEnter(Collider other)															//玩家武器上有触发器，玩家攻击碰撞后即受到攻击
	{
		if (other.tag == "Weapon" && pCtrl.attack) {											//只有玩家攻击时碰触才有效
			GameObject blood = Instantiate (bloodPre, transform.position + Vector3.up, Quaternion.identity);		//生成被攻击特效
			Destroy (blood, 1.2f);																//1.2s后销毁特效
			beAttacked = true;
		}
	}
}
