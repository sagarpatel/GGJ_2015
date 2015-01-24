using UnityEngine;
using System.Collections;

public class PathPreview : MonoBehaviour 
{
	PVA pva;
	HiggsBoson higgsBoson;

	void Start()
	{
		pva = GetComponent<PVA>();
		higgsBoson = GetComponent<HiggsBoson>();

	}


}
