using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanchpadCtrl : MonoBehaviour
{
	public GameObject startScene;
	public PlayerBallCtrl playerBallCtrl;
	public UIButtonsCtrl uibc_started;
	public GameObject cameraSwitch;
	public AudioSource audioSource;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	void OnCollisionEnter(Collision collision)
	{
		if(!uibc_started.gameStarted)
		{
			startScene.gameObject.SetActive(true);
			cameraSwitch.gameObject.SetActive(true);
			playerBallCtrl.SetRandomColor();
			audioSource.Play(0);
		}
	}
}
