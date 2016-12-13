using UnityEngine;
using System.Collections;

public class FollowHands : MonoBehaviour {

    public Transform leftHand;
    public Transform rightHand;
	
	void Update () {
        this.transform.position = Vector3.Lerp(leftHand.position, rightHand.position, .5f);
        this.transform.LookAt(leftHand);
	}
}
