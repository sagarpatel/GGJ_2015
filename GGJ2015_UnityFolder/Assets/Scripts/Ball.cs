using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	public AnimationCurve transitionCurve;
	public AnimationCurve spawnSpriteAnimationCurve;
	public float speed = 1.0f;
	public float speedIncrement = 0.05f;

	float baseSpeed;
	float maxSpeed = 2.0f;

	bool isTransitioning = false;

	GameManager gameManager;
	SpriteRenderer spriteRenderer;

	bool isLaunched = false;

	void Start()
	{
		baseSpeed = speed;
		gameManager = FindObjectOfType<GameManager>();

	}

	void OnEnable()
	{
		isLaunched = false;
		StartCoroutine(LaunchSpawnAnimation());
	}

	IEnumerator LaunchSpawnAnimation()
	{
		spriteRenderer = GetComponentInChildren<SpriteRenderer>();

		Vector3 startScale = 10.0f * Vector3.one;
		Vector3 endScale = Vector3.one;
		Color startColor = spriteRenderer.color;
		startColor.a = 0.0f;

		Color endColor = spriteRenderer.color;


		float duration = 1.0f;
		float timeCounter = 0;

		while(timeCounter < duration)
		{
			float step = spawnSpriteAnimationCurve.Evaluate(timeCounter/duration);

			spriteRenderer.transform.localScale = Vector3.Lerp(startScale, endScale, step);
			spriteRenderer.color = Color.Lerp(startColor, endColor, step);

			timeCounter += Time.deltaTime;
			yield return null;
		}

		spriteRenderer.transform.localScale = endScale;
		spriteRenderer.color = endColor;

		isLaunched = true;
	}

	void Update()
	{
		if(gameManager.isInGameplay == false)
			return;

		if(isTransitioning == false && isLaunched == true)
			transform.position += transform.up * speed * Time.deltaTime;

	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Player") == true)
		{
			Vector3 targetPos = other.gameObject.transform.position;
			Quaternion targetRot = other.gameObject.transform.rotation;

			StopAllCoroutines();
			StartCoroutine(TransitionToCenter(other.gameObject));

		}
		else if( other.gameObject.CompareTag("Outside") == true)
		{
			gameManager.BallFailed(gameObject);
		}

	}

	IEnumerator TransitionToCenter(GameObject playerGO)
	{
		isTransitioning = true;

		float timeDuration =  (1.3f * maxSpeed - speed)/maxSpeed;
		float timeCounter = 0;

		Vector3 startPos = transform.position;
		Quaternion startRot = transform.rotation;

		Vector3 targetPos = playerGO.transform.position;

		targetPos.z = startPos.z; // keep original ball depth

		PlayerControls playerControls = playerGO.GetComponent<PlayerControls>();

		while( timeCounter < timeDuration)
		{
			float step = timeCounter/timeDuration;
			step = transitionCurve.Evaluate(step);

			if(step > 0.5f)
				playerControls.isControlsLocked = true;

			transform.position = Vector3.Lerp(startPos, targetPos,step);
			transform.rotation = Quaternion.Slerp(startRot, playerGO.transform.rotation, step);

			//Debug.Log(transform.position);
			//Debug.Log( Vector3.Distance(transform.position, targetPos) );

			timeCounter += Time.deltaTime;
			yield return null;
		}

		transform.position = targetPos;
		transform.rotation = playerGO.transform.rotation;

		speed += speedIncrement;

		speed = Mathf.Clamp(speed, baseSpeed , maxSpeed);

		isTransitioning = false;
		playerControls.isControlsLocked = false;

		gameManager.IncrementMultiplier();
	}


}
