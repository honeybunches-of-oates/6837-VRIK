using UnityEngine;
using System.Collections;

public class IK_limb : MonoBehaviour {
	public Transform upperBone;
	public Transform lowerBone;
	public Transform goal;
	public Transform target;
	public Transform hint;
	public bool debug;
	public float transition = 1.0f;

	Quaternion upperBoneStartRot;
	Quaternion lowerBoneStartRot;
	Quaternion goalStartRot;

	Vector3 targetRelativeStartPos;
	Vector3 hintRelativeStartPos;

	void Start() {
		upperBoneStartRot = upperBone.rotation;
		lowerBoneStartRot = lowerBone.rotation;
		goalStartRot = goal.rotation;
		targetRelativeStartPos = target.position - upperBone.position;
		hintRelativeStartPos = hint.position - upperBone.position;
	}

	void LateUpdate () {
		CalculateIK ();
	}

	void CalculateIK () {
		
		// Calculate ikAngle variable. //
		float upperBoneLength = Vector3.Distance (upperBone.position, lowerBone.position);
		float lowerBoneLength = Vector3.Distance (lowerBone.position, goal.position);
		float limbLength = upperBoneLength + lowerBoneLength;

		float hypotenuse = upperBoneLength;
		float targetDistance = Vector3.Distance (upperBone.position, target.position);	
		targetDistance = Mathf.Min (targetDistance, limbLength - 0.0001f); //Do not allow target distance be further away than the arm's length.

		float adjacent = (Mathf.Pow (hypotenuse, 2) - Mathf.Pow (lowerBoneLength, 2) + Mathf.Pow (targetDistance, 2)) / (2 * targetDistance);

		//Debug.Log(adjacent);

		float IKAngle = Mathf.Acos (adjacent / hypotenuse) * Mathf.Rad2Deg;


		// Store pre-ik info. //
		Vector3 targetPosition = target.position;
		Vector3 hintPosition = hint.position;

		Transform upperBoneParent = upperBone.parent;
		Transform lowerBoneParent = lowerBone.parent;
		Transform goalParent = goal.parent;

		Vector3 upperBoneScale = upperBone.localScale;
		Vector3 lowerBoneScale = lowerBone.localScale;
		Vector3 goalScale = goal.localScale;

		Vector3 upperBoneLocalPosition = upperBone.localPosition;
		Vector3 lowerBoneLocalPosition = lowerBone.localPosition;
		Vector3 goalLocalPosition = goal.localPosition;

		Quaternion upperArmRotation = upperBone.rotation;
		Quaternion forearmRotation = lowerBone.rotation;
		Quaternion goalRotation = goal.rotation;


		// Reset Limb. //
		target.position = targetRelativeStartPos + upperBone.position;
		hint.position = hintRelativeStartPos + upperBone.position;
		upperBone.rotation = upperBoneStartRot;
		lowerBone.rotation = lowerBoneStartRot;
		goal.rotation = goalStartRot;


		// Work with temporaty game objects and align & parent them to the limb. //
		transform.position = upperBone.position;
		transform.LookAt(targetPosition, hintPosition - transform.position);

		GameObject upperBoneAxisCorrection = new GameObject ("upperBoneAxisCorrection");
		GameObject lowerBoneAxisCorrection = new GameObject ("lowerBoneAxisCorrection");
		GameObject goalAxisCorrection = new GameObject ("goalAxisCorrection");

		upperBoneAxisCorrection.transform.position = upperBone.position;
		upperBoneAxisCorrection.transform.LookAt(lowerBone.position, transform.root.up);
		upperBoneAxisCorrection.transform.parent = transform;
		upperBone.parent = upperBoneAxisCorrection.transform;

		lowerBoneAxisCorrection.transform.position = lowerBone.position;
		lowerBoneAxisCorrection.transform.LookAt(goal.position, transform.root.up);
		lowerBoneAxisCorrection.transform.parent = upperBoneAxisCorrection.transform;
		lowerBone.parent = lowerBoneAxisCorrection.transform;

		goalAxisCorrection.transform.position = goal.position;
		goalAxisCorrection.transform.parent = lowerBoneAxisCorrection.transform;
		goal.parent = goalAxisCorrection.transform;

		// Reset targets. //
		target.position = targetPosition;
		hint.position = hintPosition;	


		// Apply rotation for temporary game objects. //
		upperBoneAxisCorrection.transform.LookAt(target, hint.position - upperBoneAxisCorrection.transform.position);
		upperBoneAxisCorrection.transform.localRotation = (Quaternion.Euler (upperBoneAxisCorrection.transform.localRotation.eulerAngles - new Vector3 (IKAngle, 0, 0)));
		lowerBoneAxisCorrection.transform.LookAt(target, hint.position - upperBoneAxisCorrection.transform.position);
		goalAxisCorrection.transform.rotation = target.rotation;


		// Restore bones. //
		upperBone.parent = upperBoneParent;
		lowerBone.parent = lowerBoneParent;
		goal.parent = goalParent;

		upperBone.localScale = upperBoneScale;
		lowerBone.localScale = lowerBoneScale;
		goal.localScale = goalScale;

		upperBone.localPosition = upperBoneLocalPosition;
		lowerBone.localPosition = lowerBoneLocalPosition;
		goal.localPosition = goalLocalPosition;


		// Clean up temporary game objets. //
		Destroy(upperBoneAxisCorrection);
		Destroy(lowerBoneAxisCorrection);
		Destroy(goalAxisCorrection);


		// Transition. //
		transition = Mathf.Clamp01(transition);
		upperBone.rotation = Quaternion.Slerp(upperArmRotation, upperBone.rotation, transition);
		lowerBone.rotation = Quaternion.Slerp(forearmRotation, lowerBone.rotation, transition);
		goal.rotation = Quaternion.Slerp(goalRotation, goal.rotation, transition);


		// Draw Debug Gizmos. //
		if (debug){
			Debug.DrawLine(lowerBone.position, hint.position, Color.yellow);
			Debug.DrawLine(upperBone.position, target.position, Color.red);
			Debug.DrawLine(upperBone.position, lowerBone.position, Color.blue);
			Debug.DrawLine(lowerBone.position, goal.position, Color.blue);
		}
	}
}
