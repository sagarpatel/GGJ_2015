using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	public AnimationCurve transitionCurve;
	public float speed = 1.0f;

	bool isTransitioning = false;

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

		float timeDuration = 0.5f;
		float timeCounter = 0;

		Vector3 startPos = transform.position;
		Quaternion startRot = transform.rotation;

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

		isTransitioning = false;

	}


}
