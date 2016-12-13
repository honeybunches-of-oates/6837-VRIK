using UnityEngine;
using System.Collections;

public class FollowCompletely : MonoBehaviour {

	public Transform following;

	void Update() {
		this.transform.position = following.position;
		this.transform.rotation = following.rotation;
	}
}
