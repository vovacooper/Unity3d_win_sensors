using UnityEngine;
using System.Collections;

using vcTools;

public class sensorsExample : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Debug.Log("Start");
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnGUI(){
		GUI.Label (new Rect (10, 20, 300, 24), "ecceleration: " +  		SensorsWrapper.Instance.acceleration.ToString() );
		GUI.Label (new Rect (10, 35, 300, 24), "magneticHeading: " +  	SensorsWrapper.Instance.compass.magneticHeading.ToString() );
		GUI.Label (new Rect (10, 50, 300, 24), "trueHeading: " +  		SensorsWrapper.Instance.compass.trueHeading.ToString() );
		GUI.Label (new Rect (10, 65, 300, 24), "rotationRate: " +  		SensorsWrapper.Instance.gyro.rotationRate.ToString() );
		GUI.Label (new Rect (10, 80, 300, 24), "pitch: " +  			SensorsWrapper.Instance.inclinometer.pitch.ToString() );
		GUI.Label (new Rect (10, 95, 300, 24), "roll: " +  				SensorsWrapper.Instance.inclinometer.roll.ToString() );
		GUI.Label (new Rect (10, 110, 300, 24), "yaw: " +  				SensorsWrapper.Instance.inclinometer.yaw.ToString() );
		
		GUI.Label (new Rect (10, 125, 300, 24), "DeviceQuaternion: " +  SensorsWrapper.Instance.orientation.DeviceQuaternion.ToString() );
	}
}
