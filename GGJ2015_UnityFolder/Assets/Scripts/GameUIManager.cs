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


	public GameObject gameUIHolder;
	public GameObject gameOverScreenHolder;

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();

		levelReachedBaseText = levelReachedText.text;
		finalscoreBaseText = finalScoreText.text;
		highestMultiplierBaseText = highestMultiplierText.text;
		keypressBaseText = keypressText.text;
	}

	void Update()
	{
		levelText.text = levelBaseText + gameManager.GetCurrentLevel().ToString();
		scoreText.text = scoreBaseText + gameManager.GetScore().ToString();
		multiplierText.text = multiplierBaseText + gameManager.GetMultiplier().ToString();
		timeText.text = timeBaseText + gameManager.GetTimeLeft().ToString();

		levelReachedText.text = levelReachedBaseText + gameManager.GetCurrentLevel().ToString();
		finalScoreText.text = finalscoreBaseText + gameManager.GetScore().ToString();
		highestMultiplierText.text = highestMultiplierBaseText + gameManager.GetHighestMultiplier().ToString();
		keypressText.text = keypressBaseText + gameManager.GetKeyPressCount().ToString();

	}

	public void ToggleGameOverScreen(bool state)
	{
		gameOverScreenHolder.SetActive(state);
		gameUIHolder.SetActive(!state);
	}



}
