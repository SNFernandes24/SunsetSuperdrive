using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CarMotor : MonoBehaviour {
	//sets the speed to accelerate off
	public static float torque = 1500f;
	//gets center of car
	public Transform centerofMass;
	//wheelcollider of car used to control car and control meshes 
	public WheelCollider[] wheelcolliders = new WheelCollider[4];
	//tire meshes to animate rotation and movement
	public Transform[] tireMesh = new Transform[4];
	//start position when spawned for resetting car
	public static Vector3 startpos;
	//limits max speed for car
	public static float maxSpeed = 45f;
	//limits max reverse speed uses -value
	public static float maxReverse = 10f;

	//gets current speed of car
	float currentSpeed;
	//used to set speed of car with input and multiplying with torque for motorTorque
	float accelerate;
	//stores old value of torque to change back to it after boosting
	float oldTorque = torque;
	//stores old max speed to change back to it after boosting
	float oldSpeed = maxSpeed;
	//holds rotation values for tire meshes
	Quaternion quat;
	//holds position values of wheelcolliders spin tire meshes
	Vector3 pos;

	//accelerate audio source
	public AudioSource accele;
	//can keep as many audio clips as needed in array to be played from it
	public AudioClip[] audios;
	//audio source for skid to play multiple audio at same time this plays on brake
	public AudioSource skid;
	//both draw particle when mouse is held for boosting
	public ParticleSystem boost1;
	public ParticleSystem boost2;

	//gets the rigidbody of player to control
	private Rigidbody r_Body;

	//gets rigidbody, start position, applies center position to stop from flipping over, and audio source this is run only once when game starts
	void Start()
	{
		r_Body = GetComponent<Rigidbody> ();
		startpos = r_Body.transform.position;
		r_Body.centerOfMass = centerofMass.localPosition;
		accele = GetComponent<AudioSource> ();
		skid = GetComponent < AudioSource> ();
	}

	void Update()
	{
		//resets position to last checkpoint position(BUG: certain checkpoint not registering especially checkpoint3)
		if (Input.GetKeyDown(KeyCode.R)) {
			Quaternion temp = transform.rotation;
			temp.z =0;
			temp.x = 0;
			r_Body.transform.position = CheckPoint.GetActiveCheckPointPosition();
			transform.rotation = temp;
		}
		//when mouse button is held after certain speed lets you boost but at cost of handling increases max speed and torque also plays the boost particles
		if (Input.GetMouseButton (0)) {
			if(currentSpeed > maxSpeed-15)
			{
				maxSpeed =  65;
				torque = 3000;
				boost1.Play();
				boost2.Play();			
			}
		} 
		//when mouse button is released resets speed to old values and gets rid of boost particle effect
		if(Input.GetMouseButtonUp(0)){
			torque = oldTorque;
			maxSpeed = oldSpeed;
			boost1.Stop();
			boost2.Stop();
		}
		//if accelerating lets you brake, plays the audio for brakes
		if (Input.GetKey (KeyCode.Space)) {
			for (int i=0; i<4; i++) {

				wheelcolliders [i].brakeTorque = 0;
				wheelcolliders [i].motorTorque = 0;
				wheelcolliders [i].brakeTorque += accelerate * 2000;
			}
			skid.PlayOneShot(audios[2]);

		} 
		//when space is let go resets brake torque to 
		else {
			for(int i =0; i<4; i++)
			{
				wheelcolliders[i].brakeTorque = 0;

			}
		}
		SpeedControl ();
	}

	void SpeedControl() 
	{
		//limits max reverse speed
		if (currentSpeed < -maxReverse) {
			for (int i = 0; i<4; i++) {
				wheelcolliders [i].brakeTorque = 5000f;
			}
		}

		//when below certain speed play low accelerate audio
		if (accelerate > 0 && currentSpeed < 25) {
			accele.PlayOneShot(audios[0]);
		} 

		//when above certain speed play max accelerate audio
		if (accelerate > 0 && currentSpeed > 25) {
			accele.PlayOneShot(audios[1]);
		}
	}

	void FixedUpdate()
	{
		//controls the sterring wheel the value is passed from -1 to 1 depending on which side
		float steer = Input.GetAxis ("Horizontal");
		//limits max speed by normalizing
		if(r_Body.velocity.magnitude > maxSpeed)
			r_Body.velocity = r_Body.velocity.normalized * maxSpeed;
		//controlls acceleration and reverse 
		accelerate = Input.GetAxis ("Vertical");

		//sets the max rotation for either side for the colliders
		float finalAngle = steer * 45;
		wheelcolliders [0].steerAngle = finalAngle;
		wheelcolliders [1].steerAngle = finalAngle;

		//accelerates and reverses car, passes the wheelcolliders pos and rotation for tire meshes
		for (int i=0; i < 4; i++) {
			wheelcolliders[i].motorTorque = accelerate * torque;
			wheelcolliders [i].GetWorldPose (out pos, out quat);
			tireMesh [i].rotation = quat;
			tireMesh [i].position = pos;
		}

		//gets current speed either + or - depending on accelerate
		currentSpeed = accelerate * r_Body.velocity.magnitude;
			currentSpeed = Mathf.Round (currentSpeed);

	}
}

