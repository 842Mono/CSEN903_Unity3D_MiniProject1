using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleCtrl : MonoBehaviour
{

	// public GameObject player;
	private PlayerBallCtrl ctrl;

	// Use this for initialization
	void Start ()
	{
		// ctrl = player.GetComponent<PlayerBallCtrl>();8
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter()
	{
		// ctrl.IncrementScore();
		Destroy(this.gameObject);
	}
}
