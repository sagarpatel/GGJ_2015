using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
	public AudioSource outOfTime;
	public AudioSource transnfer;

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

}
