using UnityEngine;
using System.Collections;

public class StretchToFit : MonoBehaviour {

	public Transform pointA;
	public Transform pointB;
	float scaleUnstreched;
	float distanceUnstretched;
	float epsilon = .001f;

	void Start () {
		distanceUnstretched = (pointA.position - pointB.position).magnitude;
		scaleUnstreched = this.transform.localScale.y;
	}

	void Update () {
		float currentDistance = (pointA.position - pointB.position).magnitude;
		if (Mathf.Abs (currentDistance - distanceUnstretched) > epsilon) {
			this.transform.localScale = new Vector3 (this.transform.localScale.x, scaleUnstreched * (1 + (currentDistance - distanceUnstretched) / distanceUnstretched), this.transform.localScale.z);
			this.transform.position = Vector3.Lerp (pointA.position, pointB.position, .5f);
		}
	}
}
