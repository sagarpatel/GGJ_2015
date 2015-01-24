using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour 
{
	public KeyCode controlKey;

	bool isControlsLocked = false;

	public void Update()
	{

		if(isControlsLocked == true)
			return;

		if(Input.GetKeyDown(controlKey) == true)
		{
			transform.Rotate(new Vector3(0,0,-90));

		}

	}


	void OnTriggerEnter2D(Collider2D other)
	{
		isControlsLocked = true;
	}

	
	void OnTriggerExit2D(Collider2D other)
	{
		isControlsLocked = false;
	}


}
