using UnityEngine;
using System.Collections;

public class IKHandling : MonoBehaviour {

	//General Variables
	public Animator animator;

	public float IKWeight = 1;

	//Arm Variables
	public Transform leftHandTarget;
	public Transform rightHandTarget;

	public Transform leftElbowHint;
	public Transform rightElbowHint;

	//Feet Variables
	Vector3 leftFootTargetPos;
	Vector3 rightFootTargetPos;

	Quaternion leftFootTargetRot;
	Quaternion rightFootTargetRot;

	float leftFootWeight;
	float rightFootWeight;

	public Transform leftHeelTransform;
	public Transform rightHeelTransform;

	public Transform leftToeTransform;
	public Transform rightToeTransform;

	public float ankleOffset;

	void Update() {
		RaycastHit lHeelHit;
		RaycastHit rHeelHit;

		RaycastHit lToeHit;
		RaycastHit rToeHit;

		Vector3 lHeelPos = leftHeelTransform.TransformPoint (Vector3.zero);
		Vector3 rHeelPos = rightHeelTransform.TransformPoint (Vector3.zero);

		Vector3 lToePos = leftToeTransform.TransformPoint (Vector3.zero);
		Vector3 rToePos = rightToeTransform.TransformPoint (Vector3.zero);

		// Find where raycasts from feet meet the ground
		if (Physics.Raycast (lHeelPos + Vector3.up * .1f, Vector3.down, out lHeelHit, 1)) {
			leftFootTargetPos = lHeelHit.point + lHeelHit.normal * ankleOffset;
			leftFootTargetRot = Quaternion.FromToRotation (this.transform.up, lHeelHit.normal) * this.transform.rotation;
		}

		if (Physics.Raycast (rHeelPos + Vector3.up * .1f, Vector3.down, out rHeelHit, 1)) {
			rightFootTargetPos = rHeelHit.point + rHeelHit.normal * ankleOffset;
			rightFootTargetRot = Quaternion.FromToRotation (this.transform.up, rHeelHit.normal) * this.transform.rotation;
		}

		if (Physics.Raycast (lToePos + Vector3.up * .1f, Vector3.down, out lToeHit, 1)) {
			leftFootTargetPos = ((lToeHit.point + lToeHit.normal * ankleOffset) + leftFootTargetPos) / 2.0f;
			leftFootTargetRot = Quaternion.FromToRotation (this.transform.up, lToeHit.normal) * this.transform.rotation;
		}

		if (Physics.Raycast (rToePos + Vector3.up * .1f, Vector3.down, out rToeHit, 1)) {
			rightFootTargetPos = ((rToeHit.point + rToeHit.normal * ankleOffset) + rightFootTargetPos) / 2.0f;
			rightFootTargetRot = Quaternion.FromToRotation (this.transform.up, rToeHit.normal) * this.transform.rotation;
		}
	}

	void OnAnimatorIK() {

		// Retrieve Animation Weights
		leftFootWeight = animator.GetFloat ("leftFootWeight");
		rightFootWeight = animator.GetFloat ("rightFootWeight");


		// Set Feet Transforms to Feet Targets
		animator.SetIKPositionWeight (AvatarIKGoal.LeftFoot, leftFootWeight);
		animator.SetIKPositionWeight (AvatarIKGoal.RightFoot, rightFootWeight);

		animator.SetIKPosition (AvatarIKGoal.LeftFoot, leftFootTargetPos);
		animator.SetIKPosition (AvatarIKGoal.RightFoot, rightFootTargetPos);

		animator.SetIKRotationWeight (AvatarIKGoal.LeftFoot, leftFootWeight);
		animator.SetIKRotationWeight (AvatarIKGoal.RightFoot, rightFootWeight);

		animator.SetIKRotation (AvatarIKGoal.LeftFoot, leftFootTargetRot);
		animator.SetIKRotation (AvatarIKGoal.RightFoot, rightFootTargetRot);


		// Set Arm Transforms to Arm Targets
		animator.SetIKPositionWeight (AvatarIKGoal.LeftHand, IKWeight);
		animator.SetIKPositionWeight (AvatarIKGoal.RightHand, IKWeight);

		animator.SetIKPosition (AvatarIKGoal.LeftHand, leftHandTarget.position);
		animator.SetIKPosition (AvatarIKGoal.RightHand, rightHandTarget.position);

		animator.SetIKHintPositionWeight (AvatarIKHint.LeftElbow, IKWeight);
		animator.SetIKHintPositionWeight (AvatarIKHint.RightElbow, IKWeight);

		animator.SetIKHintPosition (AvatarIKHint.LeftElbow, leftElbowHint.position);
		animator.SetIKHintPosition (AvatarIKHint.RightElbow, rightElbowHint.position);

		animator.SetIKRotationWeight (AvatarIKGoal.LeftHand, IKWeight);
		animator.SetIKRotationWeight (AvatarIKGoal.RightHand, IKWeight);

		animator.SetIKRotation (AvatarIKGoal.LeftHand, leftHandTarget.rotation);
		animator.SetIKRotation (AvatarIKGoal.RightHand, rightHandTarget.rotation);
	}

}
