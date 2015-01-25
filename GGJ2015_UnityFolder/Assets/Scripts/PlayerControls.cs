using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour 
{
	public KeyCode controlKey;

	public bool isControlsLocked = false;

	GameManager gameManager;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();

	}

	public void Update()
	{

		if(gameManager.isInGameplay == false)
			return;

		if(isControlsLocked == true)
			return;

		if(Input.GetKeyDown(controlKey) == true)
		{
			transform.Rotate(new Vector3(0,0,-90));
			gameManager.IncrementKeyPresses();

		}

	}



}
