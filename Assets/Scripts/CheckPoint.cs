using UnityEngine;

public class CheckPoint : MonoBehaviour 
{
	// Indicate if the checkpoint is activated
	public bool Activated = false;

	// List with all checkpoints objects in the scene
	public static GameObject[] CheckPointsList;

	public static void LapCheck(){
		//adds 1 to counter in the script UITexts
		UITexts.counter++;
		//if counter is equal to 9 adds 1 to lap and deactivates all checkpoints for next level
		if (UITexts.counter == 9) {
			foreach (GameObject cp in CheckPointsList)
			{
				// We search the activated checkpoint to get its position
				if (cp.GetComponent<CheckPoint>().Activated)
				{
					cp.GetComponent<CheckPoint>().Activated = false;
				}
			}
			UITexts.Lapcounter++;
		}
		//gameover goes in here or anything that happens after lap count is 2-Did try to reset scene
		if(UITexts.Lapcounter == 2)
		{
				//GameOver
			//Application.LoadLevel("tempScene");
		}
	}

	/// Get position of the last activated checkpoint
	public static Vector3 GetActiveCheckPointPosition()
	{
		// If player die without activate any checkpoint, we will return a default position
		Vector3 result = CarMotor.startpos;
		
		if (CheckPointsList != null)
		{
			foreach (GameObject cp in CheckPointsList)
			{
				// We search the activated checkpoint to get its position
				if (cp.GetComponent<CheckPoint>().Activated)
				{
					result = cp.transform.position;
					break;
				}
			}
		}
		return result;
	}

	// Activate the checkpoint
	private void ActivateCheckPoint()
	{
		//if checkpoint is already activated cancel action
		if (Activated == true)
			return;
		// We activated the current checkpoint
		Activated = true;
		//run the lapcheck function to increase counter/lap
		LapCheck();
	}

	void Start()
	{
		// We search all the checkpoints in the current scene
		CheckPointsList = GameObject.FindGameObjectsWithTag("CheckPoint");
	}
	
	void OnTriggerEnter(Collider other)
	{
		// If the player passes through the checkpoint, we activate it
		if (other.tag == "Player")
		{
			ActivateCheckPoint();
		}
	}
}
