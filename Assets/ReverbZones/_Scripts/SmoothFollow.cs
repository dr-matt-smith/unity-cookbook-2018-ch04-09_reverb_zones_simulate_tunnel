using UnityEngine;
using System.Collections;

/*
This camera smoothes out rotation around the y-axis and height.
Horizontal Distance to the target is always fixed.

There are many different ways to smooth the rotation but doing it this way gives you a lot of control over how the camera behaves.

For every of those smoothed values we calculate the wanted value and the current value.
Then we smooth it using the Lerp function.
Then we apply the smoothed values to the transform's position.
*/
public class SmoothFollow : MonoBehaviour 
{
	// PUBLIC 
	
	// The target we are following
	public Transform target;
	
	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	
	// the height we want the camera to be above the target
	public float height = 5.0f;
	
	// How much we 
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
	
	
	private void LateUpdate () 
	{
		// Early out if we don't have a target
		if (!target)
			return;
		
		// Calculate the current rotation angles
		float wantedRotationAngle = target.eulerAngles.y;
		float wantedHeight = target.position.y + height;
			
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
		
		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
	
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);
	
		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0f, currentRotationAngle, 0);
		
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = target.position;
		transform.position -= currentRotation * Vector3.forward * distance;
	
		// Set the height of the camera
		Vector3  oldPos = transform.position;
		Vector3 newPosition = new Vector3(  oldPos.x, currentHeight, oldPos.z );
		transform.position = newPosition;
		
		// Always look at the target
		transform.LookAt (target);
	}
}
