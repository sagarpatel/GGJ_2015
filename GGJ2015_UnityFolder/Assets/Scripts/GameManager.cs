using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour 
{

	List<GameObject> playersList;

	float gridScale = 0.40f;
	int gridRange_x = 3;
	int gridRange_y = 2;

	List<Vector3> playersPositionList;

	void Start()
	{
		playersList = GameObject.FindGameObjectsWithTag("Player").ToList();
		playersPositionList = new List<Vector3>();

		for(int i =0; i < playersList.Count; i++)
		{
			SpawnPlayer(playersList[i]);
		}
	}

	public void SpawnPlayer(GameObject player)
	{
		// BLARGH
		while(true)
		{
			Vector3 randomPos = GenerateRandomGridPosition();
			if(playersPositionList.Contains(randomPos) == false)
			{
				player.transform.position = randomPos;
				playersPositionList.Add(randomPos);
				break;
			}

		}

	}

	Vector3 GenerateRandomGridPosition()
	{
		float randX = (float)Random.Range(-gridRange_x, gridRange_x);
		float randY = (float)Random.Range(-gridRange_y, gridRange_y);

		return gridScale * new Vector3(randX, randY);
	}


}
