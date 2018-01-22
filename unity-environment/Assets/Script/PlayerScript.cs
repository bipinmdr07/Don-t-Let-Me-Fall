using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {
	public bool rolling = true;

	void OnCollisionEnter(Collision collisionInfo){
		if (collisionInfo.gameObject.tag != null) {
			rolling = true;
		}
	}

	void OnCollisionExit(Collision collisionInfo){
		rolling = false;
	}
}
