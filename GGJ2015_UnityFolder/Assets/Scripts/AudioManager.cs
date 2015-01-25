using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour 
{
	public AudioSource outOfTime;
	public AudioSource transnfer;
	public AudioSource turn;

	public AudioSource lose;
	public AudioSource win;

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

	public void Play_Win()
	{
		StopMusic_OutOfTime();
		win.Play();
	}

	// http://www.bfxr.net/?s=2%2C0.5%2C%2C0.365%2C%2C0.1888%2C0.3%2C0.205%2C%2C0.25%2C%2C%2C%2C%2C%2C%2C%2C%2C%2C%2C0.2784%2C%2C%2C%2C%2C0.6755%2C%2C%2C0.1675%2C%2C%2C%2CmasterVolume

}
