using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour 
{
	GameManager gameManager;

	public Vector3 velocity;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
	}

	void Update()
	{

		transform.position += velocity * Time.deltaTime;
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log(other.gameObject.tag);

		if(other.gameObject.CompareTag("Ball"))
		{
			gameManager.CollectedBall();

		}
		else if(other.gameObject.CompareTag("Outside") == true)
		{
			velocity = -velocity;
		}


	}


}
