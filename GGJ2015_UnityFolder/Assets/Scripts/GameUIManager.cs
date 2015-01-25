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

	Color timeStartColor;
	Color timeEndColor;
	int counter = 0;
	bool wasRed = false;

	Vector3 normalTimeTextScale;
	Vector3 bigTimeTextScale;

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




		levelReachedText.text = levelReachedBaseText + gameManager.GetCurrentLevel().ToString();
		finalScoreText.text = finalscoreBaseText + gameManager.GetScore().ToString();
		highestMultiplierText.text = highestMultiplierBaseText + gameManager.GetHighestMultiplier().ToString();
		keypressText.text = keypressBaseText + gameManager.GetKeyPressCount().ToString();

	}

	public void ToggleGameOverScreen(bool state, bool isOutOfTime)
	{
		outOfTimeTextHolder.SetActive(isOutOfTime);
		outOfBoundsTextHolder.SetActive(!isOutOfTime);

		gameOverScreenHolder.SetActive(state);
		gameUIHolder.SetActive(!state);



	}



}
