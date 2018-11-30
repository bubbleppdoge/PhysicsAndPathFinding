using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCtrl : MonoBehaviour {

	[HideInInspector]
	public int score = 0;				//分数（击中敌人+1）
	[HideInInspector]
	public bool gameOver;				//游戏结束标志
	[HideInInspector]
	public bool gamePause;				//游戏暂停标志

	public float gameTime = 60;			//游戏时间

	public Text timeText;				//游戏界面时间显示
	public Text scoreText;				//游戏界面分数显示
	public Text hpNumText;				//游戏界面血量显示
	public Slider hpSlider;				//游戏界面血条显示
	public Image fillImage;				//游戏界面血条颜色控制

	public Text finScoreText;			//游戏结束页分数显示

	public GameObject gameOverPage;		//游戏结束页
	public GameObject pausePage;		//游戏暂停页

	public PlayerCtrl pCtrl;			//玩家控制脚本

	void Start () {
		Time.timeScale = 1;

		timeText.text = "Time: 60";
		scoreText.text = "Score: 0";
		hpNumText.text = "100";
		hpSlider.value = 100;
		finScoreText.text = "0";
		fillImage.color = Color.green;

		gameOverPage.SetActive (false);
		pausePage.SetActive (false);
		gameOver = false;
		gamePause = false;

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void Update () {
		if(!gameOver) gameTime -= Time.deltaTime;					//游戏时间减少
		if (gameTime <= 0) gameTime = 0;

		timeText.text = "Time: " + ((int)gameTime).ToString ();		//游戏界面UI显示控制
		scoreText.text = "Score: " + score.ToString ();
		hpNumText.text = pCtrl.hp.ToString();
		hpSlider.value = pCtrl.hp;
		fillImage.color = pCtrl.hp > 50 ? Color.green : pCtrl.hp > 20 ? Color.yellow : Color.red;	//颜色由血量改变而相应更改（绿黄红）

		if (Input.GetKeyDown (KeyCode.Escape)) {					//按下esc跳出暂停页
			GamePause ();
		}

		if (!gameOver && (gameTime <= 0 || pCtrl.die)) {			//玩家血量为0或者游戏时间到则游戏结束
			GameOver ();
		}
	}

	void GamePause()
	{
		Time.timeScale = 0;

		Cursor.lockState = CursorLockMode.None;					//解除鼠标隐藏
		Cursor.visible = true;

		gamePause = true;										//显示暂停页
		pausePage.SetActive (true);
	}

	void GameOver()
	{
		gameOver = true;
		gameOverPage.SetActive (true);
		finScoreText.text = score.ToString ();
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}
}
