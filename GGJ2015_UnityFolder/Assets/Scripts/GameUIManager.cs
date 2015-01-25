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

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
	}

	void Update()
	{
		levelText.text = levelBaseText + gameManager.GetCurrentLevel().ToString();
		scoreText.text = scoreBaseText + gameManager.GetScore().ToString();
		multiplierText.text = multiplierBaseText + gameManager.GetMultiplier().ToString();
		timeText.text = timeBaseText + gameManager.GetTimeLeft().ToString();
	}



}
