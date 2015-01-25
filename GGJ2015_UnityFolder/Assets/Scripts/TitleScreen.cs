using UnityEngine;
using System.Collections;

public class TitleScreen : MonoBehaviour 
{
	void Update()
	{
		if(Input.anyKeyDown)
			Application.LoadLevel(1);

	}


}
