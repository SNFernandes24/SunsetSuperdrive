using UnityEngine;
using System.Collections;

public class MoveToObject : MonoBehaviour {

	//sets this to the next node that Ai goes to
	public Transform objectToMoveTowards;
	//AI speed this is same all time no acceleration
	public float speed;
	//time it takes to turn using delta time
	public float turnSpeed;
	//quat, pos, transform and wheelcollider for wheel rotating and spinning - Not fully working
	public Transform[] tireMesh = new Transform[4];
	public WheelCollider[] wheelcolliders = new WheelCollider[4];

	Quaternion quat;
	Vector3 pos;
	
	// Update is called once per frame
	void Update () {
		//this snippet of the code doesnt belong to me. it gets turn speed by getting value between 2 and using that for its rotation while the next node is target
	    //---------------------------------------
		float ts = Mathf.Clamp01(turnSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(objectToMoveTowards.position - transform.position), ts); 
		//----------------------------------------

		//sets speed for the ai car to move forward while facing the next node to move to
		float s = Mathf.Clamp01(speed * Time.deltaTime);
		transform.position += transform.forward * s;

		//sets spin and rotate for wheel
		for (int i=0; i < 4; i++) {
			wheelcolliders [i].GetWorldPose (out pos, out quat);
			tireMesh [i].rotation = quat;
			tireMesh [i].position = pos;
		}
	}
}
