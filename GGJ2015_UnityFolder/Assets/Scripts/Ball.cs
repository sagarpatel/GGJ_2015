using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour 
{
	public float speed = 1.0f;


	void Update()
	{
		transform.position += transform.up * speed * Time.deltaTime;

	}


	void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("COLLISDING! " + other.ToString());

		if(other.gameObject.CompareTag("Player") == true)
		{
			transform.position = other.gameObject.transform.position;
			transform.up = other.gameObject.transform.up;
		}

	}

}
