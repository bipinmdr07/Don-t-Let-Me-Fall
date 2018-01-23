using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereAgent : Agent {

	public Transform gameController;

	public override List<float> CollectState(){
		List<float> state = new List<float> ();

		// as a state we need x position of player
		// end of current plank
		// begining and end of next plank
		// velocity of player
		state.Add(transform.position.x); // position of player
		state.Add(gameController.GetComponent<GameControllerScript>().endOfCurrentPlank);
		state.Add (gameController.GetComponent<GameControllerScript> ().beginingOfNextPlank);
		state.Add (gameController.GetComponent<GameControllerScript> ().endOfNextPlank);
		state.Add (transform.GetComponent<Rigidbody> ().velocity.x);

		return state;

	}

	public override void AgentStep (float[] action)
	{
		switch ((int)action [0]) {
		case 0:
			if (transform.GetComponent<PlayerScript> ().rolling) {
				gameController.GetComponent<GameControllerScript> ().Jump ();
			}
			break;
		}

		// fall of player from plank indicates the failure
		if (transform.position.y < -4){
			reward = -1f;
			done = true;
			return;
		}
			

		// if the ball is touching the plank then the agent is doing well
		if (transform.GetComponent<PlayerScript>().rolling){
			reward += 0.1f;
		}
	}

	public override void AgentReset ()
	{
		transform.position = new Vector3 (gameController.GetComponent<GameControllerScript>().endOfCurrentPlank - 15, 3, 0);
		transform.GetComponent<Rigidbody> ().velocity = new Vector3(gameController.GetComponent<GameControllerScript>().speed, 0, 0);
	}
}
