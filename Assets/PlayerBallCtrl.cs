using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBallCtrl : MonoBehaviour
{
	public float speed = 0;
	public float targetSpeed = 0;

	public GameObject cameraMain;
	public GameObject cameraTop;
	public GameObject cameraPerspective;
	private int currentCamera = 1;

	public Material red, green, blue;
	public GameObject floorPrefab;
	public GameObject CRedPrefab, CBluePrefab, CGreenPrefab;
	public GameObject changeRedPrefab, changeGreenPrefab, changeBluePrefab;
	public GameObject posLeft, posMid, posRight;
	// private List<GameObject> floors = new List<GameObject>();
	private Dictionary<int, GameObject> floorsDictionary = new Dictionary<int, GameObject>();
	private Dictionary<int, ArrayList> collectiblesDictionary = new Dictionary<int, ArrayList>();


	public int score = 0;
	public Text scoreText;


	private Rigidbody thisRigidBody;
	private Renderer thisRenderer;
	private MeshFilter thisMeshFilter;
	// private 

	private int epochInstances = 2000;
	private int z = 2000;

	private bool paused = false;

	private int highScore = 0;

	public Text gameOverText;

	public GameObject gameOverScene;

	public GameObject pauseButton;

	public GameObject changeZonePos;

	public AudioSource collectPlus;
	public AudioSource collectMinus;
	public AudioSource change;
	public AudioSource end;
	public bool muted = false;

	// Use this for initialization
	void Start ()
	{
		thisRigidBody = this.GetComponent<Rigidbody>();
		thisRenderer = this.GetComponent<Renderer>();
		thisMeshFilter = this.GetComponent<MeshFilter>();
		// Screen.orientation = ScreenOrientation.LandscapeLeft;
		for(int i = 0; i < epochInstances; i += 1000)
		{
			// floors.Add(Instantiate(floorPrefab, new Vector3(0f,0f,i), new Quaternion(0,0,0,0)));
			floorsDictionary.Add(i, Instantiate(floorPrefab, new Vector3(0f,0f,i), new Quaternion(0,0,0,0)));
		}
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		float speedIncrement = 0.01f;
		float thrust = 5;
		if(!paused)
		{
			if(Application.platform == RuntimePlatform.Android)
				transform.Translate(Input.acceleration.x * thrust, 0, 0);
			transform.Translate(0, 0, speed);
			transform.Translate(0,0,speed);
			if(speed != targetSpeed)
			{
				if(speed < targetSpeed)
					speed += speedIncrement;
				else
					speed -= speedIncrement;
				// Debug.Log(speed);
			}
		}
	}

	void FixedUpdate ()
	{
		float thrust = 5f;
		if(!paused)
		{
			if(Input.GetKey(KeyCode.LeftArrow))
				thisRigidBody.AddForce(-transform.right * thrust, ForceMode.VelocityChange);
			if(Input.GetKey(KeyCode.RightArrow))
				thisRigidBody.AddForce(transform.right * thrust, ForceMode.VelocityChange);
		}
		if(Input.GetKeyDown(KeyCode.C))
			SwitchCamera();
	}

	public void SwitchCamera ()
	{
		currentCamera = (currentCamera + 1) % 3;
		Debug.Log(currentCamera);
		switch(currentCamera)
		{
			case 0: cameraPerspective.SetActive(false); cameraMain.SetActive(true); break;
			case 1: cameraMain.SetActive(false); cameraTop.SetActive(true); break;
			case 2: cameraTop.SetActive(false); cameraPerspective.SetActive(true); break;
		}	
	}

	public void IncrementScore ()
	{
		score += 10;
		scoreText.text = score.ToString();
		highScore = score > highScore ? score : highScore;
		targetSpeed = (highScore / 50) + 2;
	}

	public void DecrementScore ()
	{
		score /= 2;
		scoreText.text = score.ToString();
		if(score == 0)
		{
			Debug.Log("Game Over");

			Pause();

			gameOverText.text = "Game Over :'C\nYour High Score : " + highScore.ToString();

			// gameOverText.gameObject.SetActive(true);
			gameOverScene.SetActive(true);
			pauseButton.SetActive(false);
			end.Play();

			// thisMeshFilter.
			// Destroy(thisMeshFilter);
			// this.gameObject.AddComponent(typeof(MeshFil))
		}
	}

	public void SetRandomColor ()
	{
		switch((int)Random.Range(0,3))
		{
			case 0: thisRenderer.material = red; break;
			case 1: thisRenderer.material = blue; break;
			case 2: thisRenderer.material = green; break;
		}
	}

	public void Redify () { thisRenderer.material = red; }
	public void Bluify () { thisRenderer.material = blue; }
	public void Greenify () { thisRenderer.material = green; }

	void GenerateEpoch () //z +- 500
	{
		Destroy(floorsDictionary[z-epochInstances]);
		if(collectiblesDictionary.ContainsKey(z-epochInstances))
		{
			ArrayList instListDestroy = collectiblesDictionary[z-epochInstances];
			foreach(var c in instListDestroy)
			{
				Destroy((GameObject)c);
			}
		}

		
		// floorsDictionary.Remove(z-5000);
		floorsDictionary.Add(z, Instantiate(floorPrefab, new Vector3(0f,0f,z), new Quaternion(0,0,0,0)));

		int changeC = (int)Random.Range(0,9);
		int zGenC = z - 500;
		ArrayList instList = new ArrayList();
		for(; zGenC < z + 500; zGenC += 100)
		{
			GameObject inst;
			if(--changeC == 0)
			{
				int randomColor = (int)Random.Range(0,3);
				Debug.Log("random:" + randomColor.ToString());
				GameObject changeZone = randomColor == 0 ? changeRedPrefab : randomColor == 1 ? changeBluePrefab : changeGreenPrefab;
				Debug.Log(changeZone);
				Vector3 pos = changeZonePos.transform.position; //= zGenC;
				pos.z = zGenC;
				inst = Instantiate(changeZone, pos, new Quaternion(0,0,0,0));
			}
			else
			{
				int randomColor = (int)Random.Range(0,3);
				GameObject collectible = randomColor == 0 ? CRedPrefab : randomColor == 1 ? CBluePrefab : CGreenPrefab;
				int randomPosition = (int)Random.Range(0,3);
				Vector3 position = randomPosition == 0 ? posLeft.transform.position : randomPosition == 1 ? posMid.transform.position : posRight.transform.position;
				position.z = zGenC;
				inst = Instantiate(collectible, position, new Quaternion(0,0,0,0));
			}
			instList.Add(inst);
		}

		collectiblesDictionary.Add(z, instList);
		z += 1000;
	}

	public void OnCollisionEnter(Collision collision)
	{
		Debug.Log(collision.collider.name);
		switch(collision.collider.name)
		{
			case "EpochCollision": GenerateEpoch(); break;
			case "CBlue(Clone)": if(Equals(thisRenderer.material.name.Substring(0, 4), "Blue")) { IncrementScore(); collectPlus.Play(); } else { DecrementScore(); collectMinus.Play(0); } break;
			case "CGreen(Clone)": if(Equals(thisRenderer.material.name.Substring(0, 5), "Green")) { IncrementScore(); collectPlus.Play();  } else { DecrementScore(); collectMinus.Play(0); } break;
			case "CRed(Clone)": if(Equals(thisRenderer.material.name.Substring(0, 3), "Red")) { IncrementScore(); collectPlus.Play(); } else { DecrementScore(); collectMinus.Play(0); } break;
			// case "CRed(Clone)": Debug.Log(this.GetComponent<Material>().name); break;
			case "ChangeRed(Clone)": thisRenderer.material = red; change.Play(0); break;
			case "ChangeBlue(Clone)": thisRenderer.material = blue; change.Play(0); break;
			case "ChangeGreen(Clone)": thisRenderer.material = green; change.Play(0); break;
		}
	}

	public void Pause ()
	{
		paused = true;
		// thisRigidBody.isKinematic = false;
		// thisRigidBody.useGravity = false;
		// this.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
		// thisRigidBody.mass = 0;
		thisRigidBody.constraints = RigidbodyConstraints.FreezeAll;
	}

	public void Unpause ()
	{
		paused = false;
		// thisRigidBody.isKinematic = true;
		// thisRigidBody.useGravity = true;
		thisRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
	}
}
