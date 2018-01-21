using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour {
	public GameObject[] planks;
	public Transform ball;

	public Camera mainCamera;

	private Rigidbody ballRigidbody;
	private Transform player;

	public int speed = 10;
	[SerializeField]
	private int jump = 400;

	private PlayerScript ps;

	private float gap_min_range = 5f;
	private float gap_max_range = 7f;

	private float plankGap; // int the range of 0.5 to 0.8
	private float endOfCurrentPlank, beginingOfNextPlank, endOfNextPlank;

	private int index;

	// Use this for initialization
	void Start () {

		index = Random.Range (0, 3);
		Instantiate (planks [index], new Vector3 (0, 0, 0), Quaternion.identity, transform);
		player = Instantiate (ball, new Vector3 (0, 2f, 0), Quaternion.identity) as Transform;
		ps = player.GetComponent<PlayerScript> ();

		ballRigidbody = player.GetComponent<Rigidbody> ();

		switch (index) {
		case 0:
			endOfCurrentPlank = 5f;
			break;
		case 1:
			endOfCurrentPlank = 7.5f;
			break;
		case 2:
			endOfCurrentPlank = 10f;
			break;
		}

		plankGap = Random.Range (gap_max_range, gap_min_range);

		index = Random.Range (0, 3);
		switch (index) {
		case 0:
			beginingOfNextPlank = plankGap + 5f + endOfCurrentPlank;
			endOfNextPlank = beginingOfNextPlank + 5f;
			break;
		case 1:
			beginingOfNextPlank = plankGap + 7.5f + endOfCurrentPlank;
			endOfNextPlank = beginingOfNextPlank + 7.5f;
			break;
		case 2:
			beginingOfNextPlank = plankGap + 10f + endOfCurrentPlank;
			endOfNextPlank = beginingOfNextPlank + 10f;
			break;
		}

		Instantiate (planks [index], new Vector3 (beginingOfNextPlank, 0, 0), Quaternion.identity, transform);
	}
	
	// Update is called once per frame
	void Update () {
		mainCamera.transform.position = new Vector3(player.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);

		// if the player go to next plank then we need to create another one next to it.
		if (player.position.x > endOfCurrentPlank){
			endOfCurrentPlank = endOfNextPlank;
			// either reuse the existing plank of create one if there is not one
			foreach (Transform passedPlank in transform){
				// disable the plank that is behind the scene
				if (passedPlank.position.x < (player.position.x - 50)){
					passedPlank.gameObject.SetActive (false);
				}
			}

			// get the random plank to show next
			index = Random.Range(0, 3);
			plankGap = Random.Range (gap_min_range, gap_max_range);

			// check if there is disabled plank with index value small, medium or large
			Transform disabledPlank = null;
			// initialize the disabledPlank if suitable plank is found for next spawn
			foreach (Transform nextPlank in this.transform) {
				if (!nextPlank.gameObject.activeSelf && index == 0 && nextPlank.tag == "SmallPlank") {
					disabledPlank = nextPlank;
				} else if (!nextPlank.gameObject.activeSelf && index == 1 && nextPlank.tag == "MediumPlank") {
					disabledPlank = nextPlank;
				} else if (!nextPlank.gameObject.activeSelf && index == 2 && nextPlank.tag == "LargePlank") {
					disabledPlank = nextPlank;
				}
			}

			switch (index) {
			// check if there are disabled small sized plank if not create one
			case 0:
				beginingOfNextPlank = endOfCurrentPlank + 5f + plankGap;
				endOfNextPlank = beginingOfNextPlank + 5f;

				if (disabledPlank != null) {
					disabledPlank.position = new Vector3 (beginingOfNextPlank, 0, 0);
					disabledPlank.gameObject.SetActive (true);
				} else {
					Instantiate (planks [index], new Vector3 (beginingOfNextPlank, 0, 0), Quaternion.identity, transform);
				}
				break;
			case 1:
				beginingOfNextPlank = endOfCurrentPlank + 7.5f + plankGap;
				endOfNextPlank = beginingOfNextPlank + 7.5f;
				if (disabledPlank != null) {
					disabledPlank.position = new Vector3 (beginingOfNextPlank, 0, 0);
					disabledPlank.gameObject.SetActive (true);
				} else {
					Instantiate (planks [index], new Vector3 (beginingOfNextPlank, 0, 0), Quaternion.identity, transform);
				}
				break;
			case 2:
				beginingOfNextPlank = endOfCurrentPlank + 10f + plankGap;
				endOfNextPlank = beginingOfNextPlank + 10f;
				if (disabledPlank != null) {
					disabledPlank.position = new Vector3 (beginingOfNextPlank, 0, 0);
					disabledPlank.gameObject.SetActive (true);
				} else {
					Instantiate (planks [index], new Vector3 (beginingOfNextPlank, 0, 0), Quaternion.identity, transform);
				}
				break;
			}
		}
			
		// game over if player fall below y = -1;
		if (player.position.y < -1) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
		
		// check if player is touching the plank or is in the air, shouldn't jump if player is not touching the plank
		if (Input.GetKeyDown (KeyCode.Space) && ps.rolling) {
			ballRigidbody.AddForce (new Vector3 (0, jump, 0));
		}

		ballRigidbody.AddForce (new Vector3 (speed, 0, 0) * Time.deltaTime);
	}
}
