using UnityEngine;
using System.Collections;

public class PlayerControls : MonoBehaviour 
{
	public KeyCode controlKey;


	public void Update()
	{

		if(Input.GetKeyDown(controlKey) == true)
		{
			transform.Rotate(new Vector3(0,0,-90));

		}

	}

}
