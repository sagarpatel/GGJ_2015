using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour 
{

	List<GameObject> playersList;

	float gridScale = 0.35f;
	int gridRange_x = 3;
	int gridRange_y = 2;

	float ballSpawnPosOffsetX = 0.35f;
	float ballSpawnPosOffsetY = 0.3f;

	List<Vector3> vacantGridPositions;
	List<Vector3> playersPositionList;
	List<Quaternion> validRotationsList;
	List<Vector3> possibleBallSpawnPositions;
	List<Quaternion> possibleBallSpawnOrientations;
	List<int> ballIndicesInUse;

	public GameObject ballPrefab;
	List<GameObject> ballsList;

	int currentLevel = 0;
	int ballsCollectedThisLevel = 0;
	int requiredBallsToNextLevel = 1;

	float score = 0;
	float currentMultiplier = 1.0f;
	float multiplierIncrement = 0.5f;
	float highestMultiplier = 1.0f;

	float timePerLevel = 20.0f;
	float timeRemainingInCurrentLevel;

	int keyPressCount = 0;

	GameUIManager gameUIManager;

	public bool isInGameplay = false;

	public Collector[] collectors;

	AudioManager audioManager;

	bool isOutOfTimePlayed = false;

	void Start()
	{
		gameUIManager = FindObjectOfType<GameUIManager>();

		playersList = GameObject.FindGameObjectsWithTag("Player").ToList();
		validRotationsList = new List<Quaternion>();

		validRotationsList.Add( Quaternion.Euler(0,0,90) );
		validRotationsList.Add( Quaternion.Euler(0,0,180) );
		validRotationsList.Add( Quaternion.Euler(0,0,270) );
		validRotationsList.Add( Quaternion.Euler(0,0,0) );

		possibleBallSpawnPositions = new List<Vector3>();
		possibleBallSpawnOrientations = new List<Quaternion>();
		ballIndicesInUse = new List<int>();
		ballsList = new List<GameObject>();

		for(int i = -gridRange_y; i < gridRange_y; i++)
		{
			Vector3 tempPos = new Vector3( -(float)gridRange_x * gridScale - ballSpawnPosOffsetX, (float)i * gridScale, 0);

			if(possibleBallSpawnPositions.Contains(tempPos) == false)
			{
				possibleBallSpawnPositions.Add(tempPos);
				possibleBallSpawnOrientations.Add( Quaternion.Euler(0,0,270) );
			}
			tempPos = new Vector3( (float)gridRange_x * gridScale + ballSpawnPosOffsetX, (float)i * gridScale, 0);
			
			if(possibleBallSpawnPositions.Contains(tempPos) == false)
			{
				possibleBallSpawnPositions.Add(tempPos);
				possibleBallSpawnOrientations.Add( Quaternion.Euler(0,0,90) );
			}
		}

		for(int i = -gridRange_x; i < gridRange_x; i++)
		{
			Vector3 tempPos = new Vector3( (float)i * gridScale, -(float)gridRange_y * gridScale - ballSpawnPosOffsetY, 0);
			
			if(possibleBallSpawnPositions.Contains(tempPos) == false)
			{
				possibleBallSpawnPositions.Add(tempPos);
				possibleBallSpawnOrientations.Add( Quaternion.Euler(0,0,0) );
			}
			
			tempPos = new Vector3( (float)i * gridScale, (float)gridRange_y * gridScale + ballSpawnPosOffsetY, 0);

			if(possibleBallSpawnPositions.Contains(tempPos) == false)
			{
				possibleBallSpawnPositions.Add(tempPos);
				possibleBallSpawnOrientations.Add( Quaternion.Euler(0,0,180) );
			}
		}

		audioManager = FindObjectOfType<AudioManager>();

		StartLevel(0);

	}

	void Update()
	{

		if(isInGameplay == false)
			return;

		timeRemainingInCurrentLevel -= Time.deltaTime;

		if(timeRemainingInCurrentLevel <= 12.0f)
		{
			if(isOutOfTimePlayed == false)
			{
				audioManager.PlayMusic_OutOfTime();
				isOutOfTimePlayed = true;
			}
		}


		if(timeRemainingInCurrentLevel <= 0)
		{

			GameOver(true);

		}

	}

	void SetCollectorsState(bool top, bool right, bool bot, bool left)
	{
		collectors[0].gameObject.SetActive(top);
		collectors[1].gameObject.SetActive(right);
		collectors[2].gameObject.SetActive(bot);
		collectors[3].gameObject.SetActive(left);
	}

	void SetCollectorsSpeed(float speedTop, float speedRight, float speedBot, float speedLeft)
	{
		Vector3 tempVec = new Vector3(1,0,0);

		collectors[0].velocity = speedTop * tempVec;
		collectors[1].velocity = speedRight * tempVec;
		collectors[2].velocity = speedBot * tempVec;
		collectors[3].velocity = speedLeft * tempVec;
	}

	void StartLevel(int level)
	{

		currentLevel = level;
		ballsCollectedThisLevel = 0;
		requiredBallsToNextLevel = currentLevel/3 +1;
		currentMultiplier = 1.0f;

		timeRemainingInCurrentLevel = timePerLevel;

		int rotLevel = level % 6;

		if(rotLevel == 0)
		{
			score = 0;
			highestMultiplier = 1.0f;
			keyPressCount = 0;

			SetCollectorsState(true, true, true, true);
		}
		else if( rotLevel == 1)
		{

			SetCollectorsState(true, true, true, true);

		}
		else if( rotLevel == 2)
		{
			SetCollectorsState(true, true, true, true);
			
		}
		else if( rotLevel == 3)
		{

			SetCollectorsState(true, true, false, true);
		}
		else if( rotLevel == 4)
		{
			SetCollectorsState(true, true, true, true);
			SetCollectorsSpeed(0.5f,0,0,0);
			
		}
		else if( rotLevel == 5)
		{
			SetCollectorsState(true, true, true, true);
			SetCollectorsSpeed(0,0.5f,0,0.5f);
		}
		else if( rotLevel == 6)
		{
			SetCollectorsState(true, true, true, true);
			SetCollectorsSpeed(0.5f,0.5f,0.5f,0.5f);

		}

		gameUIManager.ToggleGameOverScreen(false, false);

		isInGameplay = true;

		ResetPlayerPositions();
		StopCoroutine( LaunchBalls() );
		StartCoroutine( LaunchBalls() ); 
	}

	IEnumerator LaunchBalls()
	{
		for(int i  = 0; i < ballsList.Count; i++)
		{
			Destroy(ballsList[i]);
		}

		ballIndicesInUse.Clear();
		ballsList.Clear();

		int ballsToSpawnCount = requiredBallsToNextLevel;

		for(int i = 0; i < ballsToSpawnCount; i ++)
		{
			int randIndex = GenerateRandomBallPosIndex();
			GameObject newBall = (GameObject)Instantiate(ballPrefab);
			newBall.transform.position = possibleBallSpawnPositions[randIndex];
			newBall.transform.rotation = possibleBallSpawnOrientations[randIndex];

			ballsList.Add(newBall);

			yield return new WaitForSeconds(2.0f);
		}
	}

	int GenerateRandomBallPosIndex()
	{
		int tempIndex;
		/// BLAAAAAAAAAARRRRRRRRRRRRGGGGGGGGGHHHHHHHHHHH
		while(true)
		{
			tempIndex = Random.Range(0, possibleBallSpawnPositions.Count -1);
			if(ballIndicesInUse.Contains(tempIndex) == false)
			{
				ballIndicesInUse.Add(tempIndex);
				break;
			}
		}

		return tempIndex;
	}


	void ResetPlayerPositions()
	{
		playersPositionList = new List<Vector3>();
		vacantGridPositions = new List<Vector3>();
		for(int i = -gridRange_x; i <= gridRange_x; i++)
		{
			for(int j = -gridRange_y; j <= gridRange_y; j++)
			{
				Vector3 tempPos = gridScale * new Vector3((float)i, (float)j,1);
				vacantGridPositions.Add(tempPos);
			}
		}
		
		for(int i =0; i < playersList.Count; i++)
		{
			SpawnPlayer(playersList[i]);
		}

	}

	void SpawnPlayer(GameObject player)
	{
		// BLARGH
		while(true)
		{
			Vector3 randomPos = GenerateRandomGridPosition();
			if(playersPositionList.Contains(randomPos) == false)
			{
				player.transform.position = randomPos;
				playersPositionList.Add(randomPos);

				int randomRotIndex = Random.Range(0, validRotationsList.Count );
				player.transform.rotation = validRotationsList[randomRotIndex];

				// force re-inits
				player.SetActive(false);
				player.SetActive(true);

				break;
			}
		}
	}

	Vector3 GenerateRandomGridPosition()
	{
		int randomIndex = Random.Range(0, vacantGridPositions.Count -1);
		Vector3 freeGridPos = vacantGridPositions[randomIndex];
		vacantGridPositions.RemoveAt(randomIndex);
		return freeGridPos;
	}

	public IEnumerator CollectedBall(GameObject collectedBall)
	{
		ballsCollectedThisLevel +=1;
		score += currentMultiplier * 10.0f;

		
		if(ballsCollectedThisLevel >= requiredBallsToNextLevel )
		{
			isInGameplay = false;
		}

		yield return collectedBall.GetComponent<Ball>().StartCoroutine( collectedBall.GetComponent<Ball>().LaunchCollectedAnimation());

		ballsList.Remove( collectedBall );
		Destroy(collectedBall);

		if(ballsCollectedThisLevel >= requiredBallsToNextLevel )
		{
			LevelUp();
		}
	}

	void LevelUp()
	{
		StopCoroutine(LaunchLevelCompleteScreen());
		StartCoroutine(LaunchLevelCompleteScreen());
	}

	IEnumerator LaunchLevelCompleteScreen()
	{

		isInGameplay = false;
		audioManager.Play_Win();
		
		audioManager.StopMusic_OutOfTime();
		isOutOfTimePlayed = false;

		gameUIManager.ToggleLevelCompleteScreen(true);

		float cooldown = 1.0f;
		yield return new WaitForSeconds(cooldown);

		while(Input.anyKeyDown == false)
		{
			yield return null;
		}

		gameUIManager.ToggleLevelCompleteScreen(false);

		isInGameplay = true;
		StartLevel(currentLevel + 1);

	}


	public void BallFailed(GameObject failedBall)
	{
		GameOver(false);

	}

	void GameOver(bool isOutOfTime)
	{
		for(int i = 0; i < ballsList.Count; i++)
			Destroy(ballsList[i]);

		StopCoroutine(LaunchGameOverScreen(isOutOfTime));
		StartCoroutine(LaunchGameOverScreen(isOutOfTime));
	}


	IEnumerator LaunchGameOverScreen(bool isOutOfTime)
	{
		isInGameplay = false;
		gameUIManager.ToggleGameOverScreen(true, isOutOfTime);

		audioManager.Play_Lose();

		float cooldown = 1.0f;

		yield return new WaitForSeconds(cooldown);

		while(Input.anyKeyDown == false)
		{
			yield return null;
		}

		gameUIManager.ToggleGameOverScreen(false,isOutOfTime);

		isOutOfTimePlayed = false;
		StartLevel(0);
	}

	public void IncrementMultiplier()
	{
		currentMultiplier += multiplierIncrement;
		gameUIManager.MultiplierIncremented();
		
		if(currentMultiplier > highestMultiplier)
			highestMultiplier = currentMultiplier;
	}

	public void IncrementKeyPresses()
	{
		keyPressCount += 1;

	}

	public int GetCurrentLevel()
	{
		return currentLevel;
	}

	public float GetScore()
	{
		return score;
	}

	public float GetMultiplier()
	{
		return currentMultiplier;
	}

	public float GetTimeLeft()
	{
		return timeRemainingInCurrentLevel;
	}

	public float GetHighestMultiplier()
	{
		return highestMultiplier;
	}

	public int GetKeyPressCount()
	{
		return keyPressCount;
	}

	public float GetTimeRatio()
	{
		return 1.0f - timeRemainingInCurrentLevel/timePerLevel;
	}

	public float GetTimeTakenForLevel()
	{
		return timePerLevel - timeRemainingInCurrentLevel;
	}


	/*
	void OnDrawGizmosSelected() 
	{
		Gizmos.color = Color.yellow;
		for(int i = 0; i < possibleBallSpawnPositions.Count; i++)
			Gizmos.DrawSphere(possibleBallSpawnPositions[i], 0.1f);

		//Gizmos.color = Color.red;
		//for(int i = 0; i < vacantGridPositions.Count; i++)
		//	Gizmos.DrawSphere(vacantGridPositions[i], 0.1f);
	}
	*/

}
