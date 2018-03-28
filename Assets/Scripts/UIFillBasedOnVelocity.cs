using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIFillBasedOnVelocity : MonoBehaviour
{
	//measures car velocity
	public Rigidbody objectToMeasure;
	//default value
	public float maxVelocity = 10.0f;
	//played when reversing
	public bool disallowBackwards = false;
	//image to use as speed meter
	private Image image;

	// Use this for initialization
	void Start ()
	{
		image = GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//if car is moving do dot product and if greater than 0 divide cars magnitude with max velocity set earlier 
		//and fill it in with that amount otherwise keep fill at 0
		if(!disallowBackwards || Vector3.Dot(objectToMeasure.velocity, objectToMeasure.transform.forward) > 0.0f)
		{
			image.fillAmount = objectToMeasure.velocity.magnitude/maxVelocity;
		}
		else
		{
			image.fillAmount = 0.0f;
		}
	}
}
