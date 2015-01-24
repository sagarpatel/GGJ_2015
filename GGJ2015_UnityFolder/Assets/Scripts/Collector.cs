using UnityEngine;
using System.Collections;

public class Collector : MonoBehaviour 
{
	GameManager gameManager;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log(other.gameObject.tag);

		if(other.gameObject.CompareTag("Ball"))
		{
			gameManager.CollectedBall();

		}


	}


}
