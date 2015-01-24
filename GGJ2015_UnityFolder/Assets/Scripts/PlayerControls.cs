using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour 
{
	public KeyCode controlKey;

	public bool isControlsLocked = false;
	
	public void Update()
	{

		if(isControlsLocked == true)
			return;

		if(Input.GetKeyDown(controlKey) == true)
		{
			transform.Rotate(new Vector3(0,0,-90));

		}

	}



}
