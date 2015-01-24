using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour 
{
	GameManager gameManager;

	public Text levelText;
	string levelBaseText = "Level: ";

	void Start()
	{
		gameManager = FindObjectOfType<GameManager>();
	}

	void Update()
	{
		levelText.text = levelBaseText + gameManager.GetCurrentLevel().ToString();

	}



}
