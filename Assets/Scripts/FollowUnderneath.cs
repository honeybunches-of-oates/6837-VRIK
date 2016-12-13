using UnityEngine;
using System.Collections;

public class FollowUnderneath : MonoBehaviour {


	public Transform following;
	Vector3 positionOffset;
	Vector3 rotationOffset;


	void Start () {
		positionOffset = this.transform.position - following.position;
		rotationOffset = this.transform.rotation.eulerAngles - following.rotation.eulerAngles;
	}

	void FixedUpdate () {
		this.transform.position = following.position + positionOffset;
		this.transform.rotation = Quaternion.Euler (following.rotation.eulerAngles + rotationOffset);
	}
}
