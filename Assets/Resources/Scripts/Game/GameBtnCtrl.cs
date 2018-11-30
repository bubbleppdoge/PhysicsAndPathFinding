using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameBtnCtrl : MonoBehaviour {

	public RankingList rList;					//排行榜脚本
	private GameCtrl gCtrl;						//游戏控制脚本
	private CameraCtrl camCtrl;					//摄像机控制脚本

	public Text mSpeedText;						//暂停页面视角移动速度文本
	public Slider mSpeedSlider;					//速度滑动条

	void Start()
	{
		rList.gameObject.SetActive (false);

		gCtrl = GetComponent<GameCtrl> ();
		camCtrl = Camera.main.GetComponent<CameraCtrl> ();

		mSpeedText.text = "Mouse Move Speed: " + camCtrl.moveSpeed.ToString ();
		mSpeedSlider.value = camCtrl.moveSpeed;
	}

	void Update()
	{
		mSpeedText.text = "Mouse Move Speed: " + mSpeedSlider.value.ToString();		//根据滑动条的控制实施更新文本
	}

	public void SubmitBtn(InputField nameInput)										//提交按钮
	{
		rList.SetSimpleItem (nameInput.text, gCtrl.score);							//提交数据（姓名，分数）
		rList.gameObject.SetActive (true);											//显示排行榜
		rList.ShowRankingList ();													//更新排行榜数据
	}

	public void AgainBtn()															//重玩按钮
	{
		SceneManager.LoadScene(1);
	}

	public void MenuBtn()															//返回菜单按钮
	{
		SceneManager.LoadScene(0);
	}

	public void ContinueBtn(GameObject pausePage)									//继续按钮
	{
		Time.timeScale = 1;															//恢复游戏正常速度

		pausePage.SetActive (false);												//关闭暂停页
		gCtrl.gamePause = false;

		Cursor.lockState = CursorLockMode.Locked;									//隐藏指针
		Cursor.visible = false;

		camCtrl.SetSpeed ((int)mSpeedSlider.value);									//根据设置改变视角移动速度
	}
}
