using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
	public AudioSource outOfTime;
	public AudioSource transnfer;
	public AudioSource turn;

	public AudioSource lose;

	public void PlayMusic_OutOfTime()
	{
		outOfTime.Play();
	}

	public void StopMusic_OutOfTime()
	{
		outOfTime.Stop();
	}

	public void Playsound_Transfer()
	{
		transnfer.PlayOneShot(transnfer.clip);
	}

	public void Playsound_Turn()
	{
		turn.PlayOneShot(turn.clip);
	}

	public void Play_Lose()
	{
		StopMusic_OutOfTime();
		lose.Play();
	}

}
