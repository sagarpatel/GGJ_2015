using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour 
{
	GameManager gameManager;

	public Vector3 velocity;

	public Color lowColor;
	public Color highColor;
	public AnimationCurve colorLerpCurve;

	float stepCounter = 0;
	float lerpSpeed = 1.0f;
	float direction = 1.0f;

	SpriteRenderer spriteRenderer;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Update()
	{
		transform.position += velocity.x * transform.right * Time.deltaTime;


		stepCounter += direction * lerpSpeed * Time.deltaTime;
		stepCounter = Mathf.Clamp(stepCounter, 0, 1);

		if(stepCounter == 1.0f || stepCounter == 0)
			direction = -direction;

		spriteRenderer.color = Color.Lerp(lowColor, highColor, colorLerpCurve.Evaluate(stepCounter));


	}


	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log(other.gameObject.tag);

		if(other.gameObject.CompareTag("Ball"))
		{
			gameManager.StartCoroutine(gameManager.CollectedBall(other.gameObject));

		}
		else if(other.gameObject.CompareTag("Outside") == true)
		{
			velocity = -velocity;
		}


	}


}
