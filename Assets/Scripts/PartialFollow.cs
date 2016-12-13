using UnityEngine;
using System.Collections;

public class PartialFollow : MonoBehaviour {


	public Transform head;

	Vector3 headStartPos;
	Quaternion headStartRot;

	public float zRotationTracking;
	public float leanAngle;
	Vector3 posOffset;
	Vector3 rotOffset;
	public float speed;
	public float rotSpeed;
	 
	void Start () {
		headStartPos = head.position;
		headStartRot = head.localRotation;
		posOffset = this.transform.position - head.position;
		rotOffset = this.transform.rotation.eulerAngles - head.rotation.eulerAngles;
	}

	void FixedUpdate () {
		float posStep = Time.fixedDeltaTime * speed;
		float rotStep = Time.fixedDeltaTime * rotSpeed;

		Vector3 targetPosition = CalculateTargetPosition ();
		this.transform.position = CalculateTargetPosition ();
		//this.transform.position = Vector3.MoveTowards (this.transform.position, targetPosition, posStep);

		CalculateTargetRotation ();
		//Quaternion targetRotation = CalculateTargetRotation ();
		//this.transform.rotation = Quaternion.RotateTowards (this.transform.rotation, targetRotation, rotStep);
	}

	Vector3 CalculateTargetPosition () {
		return head.position + posOffset;
	}

	void CalculateTargetRotation () {
		this.transform.rotation = Quaternion.Euler (head.rotation.eulerAngles + rotOffset);
		float angleDiff = headStartRot.eulerAngles.x - head.localRotation.eulerAngles.x;
		while (Mathf.Abs(angleDiff) > 180) {
			angleDiff -= Mathf.Sign (angleDiff) * 360f;
		}
		if (Mathf.Abs(angleDiff) < leanAngle)
			return;
		else if (angleDiff > 0) {
			this.transform.RotateAround (head.position, -head.TransformVector (Vector3.right), Mathf.Abs(angleDiff) - leanAngle);
		} else if (angleDiff < 0) {
			Debug.Log ("this one");
			this.transform.RotateAround (head.position, head.TransformVector (Vector3.right), Mathf.Abs(angleDiff) - leanAngle);
		}
	}
}
