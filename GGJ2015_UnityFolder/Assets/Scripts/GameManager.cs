using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour 
{

	List<GameObject> playersList;

	float gridScale = 0.30f;
	int gridRange_x = 3;
	int gridRange_y = 2;

	List<Vector3> vacantGridPositions;
	List<Vector3> playersPositionList;
	List<Quaternion> validRotationsList;

	public GameObject ballPrefab;

	int currentLevel = 0;
	int ballsCollectedThisLevel = 0;

	void Start()
	{
		playersList = GameObject.FindGameObjectsWithTag("Player").ToList();
		validRotationsList = new List<Quaternion>();

		validRotationsList.Add( Quaternion.Euler(0,0,90) );
		validRotationsList.Add( Quaternion.Euler(0,0,180) );
		validRotationsList.Add( Quaternion.Euler(0,0,270) );
		validRotationsList.Add( Quaternion.Euler(0,0,0) );

		ResetPlayerPositions();
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

	public void CollectedBall()
	{
		ballsCollectedThisLevel +=1;

		if(ballsCollectedThisLevel >= currentLevel )
		{
			LevelUp();
		}
	}

	void LevelUp()
	{
		currentLevel += 1;
		ballsCollectedThisLevel = 0;

		ResetPlayerPositions();

	}






	/*
	void OnDrawGizmosSelected() 
	{
		Gizmos.color = Color.yellow;
		for(int i = 0; i < playersPositionList.Count; i++)
			Gizmos.DrawSphere(playersPositionList[i], 0.1f);

		Gizmos.color = Color.red;
		for(int i = 0; i < vacantGridPositions.Count; i++)
			Gizmos.DrawSphere(vacantGridPositions[i], 0.1f);
	}
	*/


}
