using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
	const float distanceAway = 5; // distance from the back of the craft
	const float distanceUp = 2; // distance above the craft
	const float smooth = 3; // how smooth the camera movement is
	
    [SerializeField]
	Transform follow;
	
	void LateUpdate ()
	{
		// setting the target position to be the correct offset from the hovercraft
		Vector3 targetPosition = follow.position + Vector3.up * distanceUp - follow.forward * distanceAway;
		// making a smooth transition between it's current position and the position it wants to be in
		transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smooth);
		// make sure the camera is looking the right way!
		transform.LookAt(follow);
	}
}
