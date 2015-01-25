using UnityEngine;
using System.Collections;

public class ChangeScreens : MonoBehaviour 
{
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape) == true)
		{

			Application.LoadLevel(0);

		}

	}

}
