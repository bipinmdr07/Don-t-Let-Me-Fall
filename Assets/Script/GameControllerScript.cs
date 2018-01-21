using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour {
	public GameObject[] planks;
	public Transform ball;
	private Rigidbody ballRigidbody;
	private Transform player;

	public int speed = 10;
	[SerializeField]
	private int jump = 100;

	private float gap_min_range = 5f;
	private float gap_max_range = 7f;

	private float plankGap; // int the range of 0.5 to 0.8
	private float endOfCurrentPlank, beginingOfNextPlank;

	private int index;

	// Use this for initialization
	void Start () {

		index = Random.Range (0, 3);
		Instantiate (planks [index], new Vector3 (0, 0, 0), Quaternion.identity);
		player = Instantiate (ball, new Vector3 (0, 2f, 0), Quaternion.identity) as Transform;

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
		Debug.Log (plankGap);

		index = Random.Range (0, 3);
		switch (index) {
		case 0:
			beginingOfNextPlank = plankGap + 5f + endOfCurrentPlank;
			break;
		case 1:
			beginingOfNextPlank = plankGap + 7.5f + endOfCurrentPlank;
			break;
		case 2:
			beginingOfNextPlank = plankGap + 10f + endOfCurrentPlank;
			break;
		}

		Instantiate (planks [index], new Vector3 (beginingOfNextPlank, 0, 0), Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {

		if (player.position.y < 0) {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
		

		if (Input.GetKeyDown (KeyCode.Space) && player.position.y <= 2f) {
			ballRigidbody.AddForce (new Vector3 (0, jump, 0));
		}

		ballRigidbody.AddForce (new Vector3 (speed, 0, 0) * Time.deltaTime);
	}
}
