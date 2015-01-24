using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	public AnimationCurve transitionCurve;
	public float speed = 1.0f;
	public float speedIncrement = 0.05f;

	float baseSpeed;
	float maxSpeed = 2.0f;

	bool isTransitioning = false;

	void Start()
	{
		baseSpeed = speed;
	}

	void Update()
	{
		if(isTransitioning == false)
			transform.position += transform.up * speed * Time.deltaTime;

	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Player") == true)
		{
			Vector3 targetPos = other.gameObject.transform.position;
			Quaternion targetRot = other.gameObject.transform.rotation;

			StopAllCoroutines();
			StartCoroutine(TransitionToCenter(targetPos, targetRot));

		}

	}

	IEnumerator TransitionToCenter(Vector3 targetPos, Quaternion targetRot)
	{
		isTransitioning = true;

		float timeDuration =  (1.3f * maxSpeed - speed)/maxSpeed;
		float timeCounter = 0;

		Vector3 startPos = transform.position;
		Quaternion startRot = transform.rotation;

		targetPos.z = startPos.z; // keep original ball depth

		while( timeCounter < timeDuration)
		{
			float step = timeCounter/timeDuration;
			step = transitionCurve.Evaluate(step);

			transform.position = Vector3.Lerp(startPos, targetPos,step);
			transform.rotation = Quaternion.Slerp(startRot, targetRot, step);

			timeCounter += Time.deltaTime;
			yield return null;
		}

		transform.position = targetPos;
		transform.rotation = targetRot;

		speed += speedIncrement;

		speed = Mathf.Clamp(speed, baseSpeed , maxSpeed);

		isTransitioning = false;
	}


}
