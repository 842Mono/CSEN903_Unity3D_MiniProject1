using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIButtonsCtrl : MonoBehaviour
{
	public GameObject startScene;
	public GameObject inGameScene;
	public GameObject optionsScene;
	public Button startGame;
	public Button switchCamera;
	public Button pauseButton;
	public Button optionsButton;
	public Button settingsBackButton;
	public Button restart;
	public Button restartGO;
	public GameObject playerCtrl;
	public PlayerBallCtrl ctrl;

	public GameObject score;

	public bool gameStarted = false;
	public AudioSource paused;
	public AudioSource ingame;
	public AudioSource collect;
	public AudioSource sWitch;
	public AudioSource end;
	// public AudioSource[] allAudio;
	public Button mute;
	
	void Start ()
	{
		startGame.onClick.AddListener(startGameOnClick);
		switchCamera.onClick.AddListener(SwitchCamera);
		pauseButton.onClick.AddListener(Pause);
		settingsBackButton.onClick.AddListener(SettingsBack);
		optionsButton.onClick.AddListener(Options);
		restartGO.onClick.AddListener(LoadScene);
		restart.onClick.AddListener(LoadScene);
		mute.onClick.AddListener(Mute);
		ctrl = playerCtrl.GetComponent<PlayerBallCtrl>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void startGameOnClick()
	{
		startScene.gameObject.SetActive(false);
		score.SetActive(true);
		inGameScene.SetActive(true);
		paused.Stop();
		gameStarted = true;
		ingame.Play(0);
		ctrl.targetSpeed = 2;
	}

	void SwitchCamera ()
	{
		ctrl.SwitchCamera();
	}

	void Pause ()
	{
		inGameScene.SetActive(false);
		optionsScene.SetActive(true);
		ctrl.Pause();
	}

	void SettingsBack ()
	{
		optionsScene.SetActive(false);
		if(!gameStarted)
		{
			startScene.SetActive(true);
		}
		else //unpause
		{
			inGameScene.SetActive(true);
			ctrl.Unpause();
		}
	}

	void Options ()
	{
		startScene.SetActive(false);
		optionsScene.SetActive(true);
	}

	void LoadScene ()
	{
		SceneManager.LoadScene("SampleScene", LoadSceneMode.Single);
	}

	void Mute ()
	{
		paused.mute = !paused.mute;
		ingame.mute = !ingame.mute;
		collect.mute = !sWitch.mute;
		sWitch.mute = !sWitch.mute;
		end.mute = !end.mute;
		// Debug.Log("here");
		// if(ctrl.muted)
		// {
		// 	//unmute
		// 	ctrl.muted = false;
		// 	if(gameStarted)
		// 		ingame.UnPause();
		// 	else
		// 		paused.UnPause();
		// }
		// else
		// {
		// 	//mute
		// 	ctrl.muted = true;
		// 	if(gameStarted)
		// 		ingame.Pause();
		// 	else
		// 		paused.Pause();
		// }
	}
}
