using UnityEngine;
using System.Collections;

public class ProximityScaling : MonoBehaviour 
{

	public AnimationCurve scaleCurve;

	bool isScaling = false;
	
	Vector3 baseScale;
	Transform ballTransform;
	float maxDiff;
	Vector3 maxScale;

	void Start()
	{
		baseScale = transform.localScale;
		maxScale = 1.5f * baseScale;
	}

	void OnEnable()
	{
		isScaling = false;
	}
	
	public void Update()
	{
		if(isScaling == true)
		{
			float currentDiff = Vector2.Distance(ballTransform.position, transform.position);
			float step = (maxDiff - currentDiff)/maxDiff;
			step = scaleCurve.Evaluate(step);
			transform.localScale = Vector3.Lerp(baseScale, maxScale, step);
		}
	}
	
	
	void OnTriggerEnter2D(Collider2D other)
	{

		if(other.gameObject.CompareTag("Ball") == true)
		{
			isScaling = true;
			ballTransform = other.transform;
			maxDiff = Vector2.Distance(ballTransform.position, transform.position);
		}
	}
	
	
	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.CompareTag("Ball") == true && other.transform == ballTransform)
		{
			isScaling = false;
			transform.localScale = baseScale;
		}
	}


}
