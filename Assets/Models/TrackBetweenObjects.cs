using UnityEngine;
using System.Collections;

public class TrackBetweenObjects : MonoBehaviour {

	public Transform object1;
	public Transform object2;
	public Transform object3;
	public Transform object3Anchored;

	public Transform shoulderTarget;
	public float maxArmLength;
	Quaternion startLocalRot;
	Vector3 startLocalPos;

	public Vector3 influence = new Vector3 (.25f, .25f, .15f);
    public float yRotInfluence;
    public float zRotInfluence;
	public float zRotMaxDegrees = 60f;
	public float yRotMaxDegrees = 45f;

	void AdjustTargetRotation() {
		shoulderTarget.localRotation = startLocalRot;
		Vector3 tempPos = shoulderTarget.localPosition;
		shoulderTarget.localPosition = startLocalPos;

		float yDistFromCenter = Mathf.Abs (object3Anchored.InverseTransformPoint (object1.position).y) - Mathf.Abs(object3Anchored.InverseTransformPoint (object2.position).y);
		//Debug.Log ("Y Distance: " + yDistFromCenter);
		float ratioForYRot = yDistFromCenter / (2 * maxArmLength);
		float angleForYRot = -Mathf.Atan (Mathf.Abs (ratioForYRot)) * zRotMaxDegrees / 360f * Mathf.Sign (ratioForYRot);

		float zDistFromCenter = Mathf.Abs (object3Anchored.InverseTransformPoint (object1.position).z) - Mathf.Abs(object3Anchored.InverseTransformPoint (object2.position).z);
		//Debug.Log ("Z Distance: " + zDistFromCenter);
		float ratioForZRot = zDistFromCenter / (2 * maxArmLength);
		float angleForZRot = -Mathf.Atan (Mathf.Abs(ratioForZRot)) * zRotMaxDegrees / 360f * Mathf.Sign (ratioForZRot);

		if (float.IsNaN (angleForYRot)) {
			angleForYRot = 0;
		}

		if (float.IsNaN (angleForZRot)) {
			angleForZRot = 0;
		}

		//Debug.Log ("Rotating: " + new Vector3 (0, angleForYRot * Mathf.Rad2Deg, angleForZRot * Mathf.Rad2Deg).ToString ());

		shoulderTarget.localRotation = (Quaternion.Euler(new Vector3(0, angleForYRot * Mathf.Rad2Deg * yRotInfluence, angleForZRot * Mathf.Rad2Deg * zRotInfluence)));
		shoulderTarget.localPosition = tempPos;
	}

	void Start () {
		startLocalRot = shoulderTarget.localRotation;
		startLocalPos = shoulderTarget.localPosition;

		Vector3 temp = Vector3.Lerp (object1.position, object2.position, .5f);
		float x = Mathf.Lerp (object3.position.x, temp.x, influence.x);
		float y = Mathf.Lerp (object3.position.y, temp.y, influence.y);
		float z = Mathf.Lerp (object3.position.z, temp.z, influence.z);
		this.transform.position = new Vector3 (x, y, z);
	}

	void Update () {
		Vector3 temp = Vector3.Lerp (object1.position, object2.position, .5f);
		float x = Mathf.Lerp (object3.position.x, temp.x, influence.x);
		float y = Mathf.Lerp (object3.position.y, temp.y, influence.y);
		float z = Mathf.Lerp (object3.position.z, temp.z, influence.z);
		this.transform.position = new Vector3 (x, y, z);

		AdjustTargetRotation ();
	}
}
