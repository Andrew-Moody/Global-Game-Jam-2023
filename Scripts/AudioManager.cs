using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public static AudioManager Instance;

	public List<AudioClip> Clips;

    private AudioSource _source;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		_source = GetComponent<AudioSource>();
	}

	private void Start()
	{
		
	}

	public void PlaySound(int id)
	{
		_source.PlayOneShot(Clips[id]);
	}
	
}
