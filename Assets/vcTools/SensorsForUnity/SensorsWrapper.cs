using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;

using System.Diagnostics;
namespace vcTools
{
public class SensorsWrapper
{
	// -- Acceleration
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetAccelerationReadingX ();
    
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetAccelerationReadingY ();
	
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetAccelerationReadingZ ();
	
		
	// -- Gyro
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetGyroReadingX ();
    
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetGyroReadingY ();
	
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetGyroReadingZ ();
	
	
	
	// -- Compass
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetCompassHeadingMagneticNorth ();
    
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetCompassHeadingTrueNorth ();

	
	// -- Inclinometer
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetInclinometerPitch ();
    
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetInclinometerYaw ();
	
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern double GetInclinometerRoll ();
	
	// -- Orientation
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern void GetOrientationRotationMatrix (float[] matrix);
	
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern float GetOrientationQuaternionX ();
	
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern float GetOrientationQuaternionY ();
	
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern float GetOrientationQuaternionZ ();
	
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern float GetOrientationQuaternionW ();
	
	
	// -- General
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern void Init ();
	
	[DllImport("ReadSensors", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
	private static extern void Finish ();
	
	private SensorsWrapper ()
	{
		Init();	
	}
	
	~SensorsWrapper ()
	{
		Finish ();
	}
	
	public static SensorsWrapper Instance = new SensorsWrapper ();
	
	public Vector3 acceleration {	
		get {
			return new Vector3 ((float)GetAccelerationReadingX (), (float)GetAccelerationReadingY (), (float)GetAccelerationReadingZ ());
		}
	}
	
	
	// ---
	
	public class iGyroscope
	{	    
		public Vector3 rotationRate;	 
	};

	public iGyroscope gyro {	
		get {
			iGyroscope gyro = new iGyroscope ();
			gyro.rotationRate = new Vector3 ((float)GetGyroReadingX (), (float)GetGyroReadingY (), (float)GetGyroReadingZ ());			
			return gyro;
		}
	}
	
	// --
	
	public class iCompass
	{	    
		public float magneticHeading;
		public float trueHeading;	 
	};
	
	public iCompass compass {	
		get {
			iCompass compass = new iCompass ();
			compass.magneticHeading = (float)(GetCompassHeadingMagneticNorth ());
			compass.trueHeading = (float)(GetCompassHeadingTrueNorth ());
			return compass;
		}
	}
	
	// -- Inclinometer 
		
	public class iInclinometer
	{	    
		public float pitch;
		public float yaw;
		public float roll;	 
	};
	
	public iInclinometer inclinometer {	
		get {
			iInclinometer inclinometer = new iInclinometer ();
			inclinometer.pitch = (float)(GetInclinometerPitch ());
			inclinometer.yaw = (float)(GetInclinometerYaw ());
			inclinometer.roll = (float)(GetInclinometerRoll ());
			return inclinometer;
		}
	}
	
	// -- Inclinometer 
		
	public class iOrientation
	{	     	
		public Quaternion DeviceQuaternion;
		public float[] OrientaionMatrix;
	};
	
	void NormalizeQuaternion (ref Quaternion q)
	{
		float sum = 0;
		for (int i = 0; i < 4; ++i) {
			sum += q [i] * q [i];
		}
		float magnitudeInverse = 1f / Mathf.Sqrt (sum);
		for (int i = 0; i < 4; ++i) {
			q [i] *= magnitudeInverse;   
		}
	}
	
	private void MSFix (ref Quaternion rotationQuaternion) //TODO fix
	{
		NormalizeQuaternion (ref rotationQuaternion);

		// get a translation, rotation and scaling matrix.
		Matrix4x4 transformationMatrix = Matrix4x4.TRS (Vector3.zero, rotationQuaternion, Vector3.one);
		Matrix4x4 transformMatrixHelper = new Matrix4x4 ();
		transformMatrixHelper [0, 0] = -1;  //-1
		transformMatrixHelper [1, 1] = -1;  //-1
		transformMatrixHelper [2, 2] = 1;   //1
		transformMatrixHelper [3, 3] = 1;   //1
		
		//performing transformation 
		transformMatrixHelper = transformMatrixHelper.inverse * transformationMatrix * transformMatrixHelper;
			
		//Get Qunaternion from Matrix
		rotationQuaternion = Quaternion.LookRotation (transformMatrixHelper.GetColumn (2), transformMatrixHelper.GetColumn (1));
		
		rotationQuaternion = Quaternion.AngleAxis (90f, new Vector3 (1f, 0f, 0f)) * rotationQuaternion;
		rotationQuaternion = Quaternion.AngleAxis (180f, new Vector3 (0f, 1f, 0f)) * rotationQuaternion;
	}
	
	public iOrientation orientation {	
		get {
			iOrientation orientation = new iOrientation ();
			orientation.DeviceQuaternion = new Quaternion (GetOrientationQuaternionX (), 
															GetOrientationQuaternionY (), 
															GetOrientationQuaternionZ (), 
															GetOrientationQuaternionW ());
			// fixing microsoft convention to match Unity's one
			MSFix (ref orientation.DeviceQuaternion);
			
			orientation.OrientaionMatrix = new float[9];
			GetOrientationRotationMatrix (orientation.OrientaionMatrix);
				
			return orientation;
		}
	}
}
}