using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour 
{
	GameManager gameManager;

	public Text levelText;
	string levelBaseText = "Level: ";

	public Text scoreText;
	string scoreBaseText = "Score: ";

	public Text multiplierText;
	string multiplierBaseText = "Multiplier: x";

	public Text timeText;
	string timeBaseText = "Time left: ";

	// game over screen

	public Text levelReachedText;
	string levelReachedBaseText;
	public Text finalScoreText;
	string finalscoreBaseText;
	public Text highestMultiplierText;
	string highestMultiplierBaseText;
	public Text keypressText;
	string keypressBaseText;

	public GameObject outOfTimeTextHolder;
	public GameObject outOfBoundsTextHolder;

	public GameObject gameUIHolder;
	public GameObject gameOverScreenHolder;
	public GameObject levelCompleteScreenHolder;

	Color timeStartColor;
	Color timeEndColor;
	int counter = 0;
	bool wasRed = false;

	Vector3 normalTimeTextScale;
	Vector3 bigTimeTextScale;

	float multiplierScaleCooldown = 3.0f;
	float multiplierScaleTimeCounter = 0;
	public AnimationCurve multiplierScaleCurve;
	Color normalMultiplierColor;
	Vector3 normalMultiplierScale;



	public Text clearedLevelText;
	string cleareddLevelBaseText;
	public Text cumulativeScoreText;
	string cumulativeScoreBaseText;
	public Text timeTakenText;
	string timeTakenBaseText;


	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();

		levelReachedBaseText = levelReachedText.text;
		finalscoreBaseText = finalScoreText.text;
		highestMultiplierBaseText = highestMultiplierText.text;
		keypressBaseText = keypressText.text;

		timeStartColor = timeText.color;
		timeEndColor = Color.red;

		normalTimeTextScale = timeText.transform.localScale;
		bigTimeTextScale = 1.5f * normalTimeTextScale;
		normalMultiplierColor = multiplierText.color;
		normalMultiplierScale = multiplierText.transform.localScale;

		cleareddLevelBaseText = clearedLevelText.text;
		cumulativeScoreBaseText = cumulativeScoreText.text;
		timeTakenBaseText = timeTakenText.text;
	}

	void Update()
	{
		levelText.text = levelBaseText + gameManager.GetCurrentLevel().ToString();
		scoreText.text = scoreBaseText + gameManager.GetScore().ToString();
		multiplierText.text = multiplierBaseText + gameManager.GetMultiplier().ToString();
		timeText.text = timeBaseText + gameManager.GetTimeLeft().ToString();

		float progress = gameManager.GetTimeRatio();
		timeText.color = Color.Lerp(timeStartColor, timeEndColor, progress );

		if(progress > 0.75f)
		{
			timeText.transform.localScale = bigTimeTextScale;
			counter ++;
			if(counter %8 == 0)
			{
				if(wasRed == true)
					timeText.color = Color.black;
				else
					timeText.color = Color.red;

				wasRed = !wasRed;
			}
		}
		else
		{
			counter = 0;
			timeText.transform.localScale = normalTimeTextScale;
		}


		multiplierScaleTimeCounter += Time.deltaTime;
		multiplierScaleTimeCounter = Mathf.Clamp(multiplierScaleTimeCounter, 0, multiplierScaleCooldown);

		float multiplierStep = 1.0f - multiplierScaleTimeCounter/multiplierScaleCooldown;
		multiplierStep = multiplierScaleCurve.Evaluate(multiplierStep);
		multiplierText.transform.localScale = Vector3.Lerp(normalMultiplierScale, 1.25f * normalMultiplierScale, multiplierStep);
		multiplierText.color = Color.Lerp(normalMultiplierColor, Color.white, multiplierStep);

		levelReachedText.text = levelReachedBaseText + gameManager.GetCurrentLevel().ToString();
		finalScoreText.text = finalscoreBaseText + gameManager.GetScore().ToString();
		highestMultiplierText.text = highestMultiplierBaseText + gameManager.GetHighestMultiplier().ToString();
		keypressText.text = keypressBaseText + gameManager.GetKeyPressCount().ToString();


		clearedLevelText.text = cleareddLevelBaseText + gameManager.GetCurrentLevel().ToString();
		cumulativeScoreText.text = cumulativeScoreBaseText + gameManager.GetScore().ToString();
		timeTakenText.text = timeTakenBaseText + gameManager.GetTimeTakenForLevel().ToString() + " seconds";


	}

	public void MultiplierIncremented()
	{
		multiplierScaleTimeCounter = 0;
	}

	public void ToggleGameOverScreen(bool state, bool isOutOfTime)
	{
		outOfTimeTextHolder.SetActive(isOutOfTime);
		outOfBoundsTextHolder.SetActive(!isOutOfTime);

		gameOverScreenHolder.SetActive(state);
		gameUIHolder.SetActive(!state);
	}

	public void ToggleLevelCompleteScreen(bool state)
	{
		levelCompleteScreenHolder.SetActive(state);
		gameUIHolder.SetActive(!state);
	}



}
