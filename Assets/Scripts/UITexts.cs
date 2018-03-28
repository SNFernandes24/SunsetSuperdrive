using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UITexts : MonoBehaviour {

	//sets text for checkpoint, laps, time and best time
	public Text checkpoint;
	public Text Laps;
	public Text time;
	public Text Btime;

	//checkpoint counter and lap counter altered in checkpoint script too
	public static int counter = 0;
	public static int Lapcounter = 0;

	//stores time
	float t;
	//stores minutes and seconds in string
	string minutes;
	string seconds;
	//minValue and secValue hold converted string value BestMinute and BestSecond are used to compare for best time they each start with 99 so first is guaranteed
	int BestMinute = 99;
	int BestSeconds = 99;
	int minValue;
	int secValue;

	// Update is called once per frame
	void Update () {
		//prints and updates counter value to show checkpoint
		updateCheckpoints ();
		//prints and updates lapcounter value to show laps
		updateLaps();
		//start time
		t += Time.deltaTime;
		//get minutes
		minutes = Mathf.Floor(t / 60).ToString("00");
		//get seconds
		seconds = Mathf.Floor(t % 60).ToString("00");
		//show timer in MM:SS format
		time.text = minutes + ":"+ seconds;
		//convert string to int
		minValue = int.Parse (minutes);
		secValue = int.Parse (seconds);
		//if counter is 9 run bestTime check function, reset time, minValue and secValue
		if (counter == 9) {
			bestTime();
			t=0;
			minValue = 0;
			secValue =0;
		}
	}

	void bestTime(){
		//if minValue and secValue is lower than BestMinute and BestSeconds pass values to them and print best time update if needed and reset checkPoint counter
		if (BestMinute > minValue && BestSeconds > secValue) {
			BestMinute = minValue;
			BestSeconds = secValue;
			Btime.text = "Best: "+BestMinute+":"+BestSeconds;
		}
		counter = 0;
	}

	void updateCheckpoints()
	{
		checkpoint.text = "Checkpoints: " + counter + "/9";
	}

	void updateLaps()
	{
		Laps.text = "Lap: " + Lapcounter + "/2";
	}
}
