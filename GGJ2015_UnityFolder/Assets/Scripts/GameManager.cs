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

	float timePerLevel = 20.0f;
	float timeRemainingInCurrentLevel;

	void Start()
	{
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

		StartLevel(0);

	}

	void Update()
	{

		timeRemainingInCurrentLevel -= Time.deltaTime;

		if(timeRemainingInCurrentLevel <= 0)
			GameOver();

	}



	void StartLevel(int level)
	{
		currentLevel = level;
		ballsCollectedThisLevel = 0;
		requiredBallsToNextLevel = currentLevel/3 +1;
		currentMultiplier = 1.0f;

		timeRemainingInCurrentLevel = timePerLevel;

		if(level == 0)
			score = 0;

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

	public void CollectedBall(GameObject collectedBall)
	{
		ballsCollectedThisLevel +=1;
		score += currentMultiplier * 10.0f;

		ballsList.Remove( collectedBall );
		Destroy(collectedBall);

		if(ballsCollectedThisLevel >= requiredBallsToNextLevel )
		{
			LevelUp();
		}
	}

	void LevelUp()
	{
		StartLevel(currentLevel + 1);
	}


	public void BallFailed(GameObject failedBall)
	{


		GameOver();


	}

	void GameOver()
	{
		for(int i = 0; i < ballsList.Count; i++)
			Destroy(ballsList[i]);

		StartLevel(0);
	}

	public void IncrementMultiplier()
	{
		currentMultiplier += multiplierIncrement;
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
