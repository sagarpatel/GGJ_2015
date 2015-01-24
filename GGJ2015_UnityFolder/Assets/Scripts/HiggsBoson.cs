using UnityEngine;
using System.Collections;
using System.Linq;

public class HiggsBoson : MonoBehaviour 
{
	public bool isStatic = false;
	public float mass = 1.0f;

	float gravityConstant = 10.0f;

	HiggsBoson[] gravitySources;
	PVA pva;

	void Start()
	{
		gravitySources = FindObjectsOfType<HiggsBoson>().Where( h => h.isStatic).ToArray();
		Debug.Log(gravitySources.Length);

		if(isStatic == false)
		{
			pva = GetComponent<PVA>();
		}
	}


	void Update()
	{
		if(isStatic == false)
		{
			Vector3 forces = new Vector3();
			for(int i = 0; i < gravitySources.Length; i++)
			{
				Vector3 diff = gravitySources[i].transform.position - transform.position;
				float distSquared = Vector3.SqrMagnitude(diff);
				Vector3 direction = diff.normalized;
				Vector3 force = gravityConstant * mass * gravitySources[i].mass * direction / Mathf.Sqrt( distSquared);
				forces += force;
			}
			Debug.Log(forces);
			pva.acceleration += forces * Time.deltaTime;
		}
	}
}
