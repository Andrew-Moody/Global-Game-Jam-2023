using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance;

	public AudioSource ThemeSong;

	public Boss BossPrefab;

	public Vector3[] BossLocs;

	private int _bossesLeft;

	private Player _player;

	private int _wave;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
	}

	private void Start()
	{
		_player = Player.Instance;

		Time.timeScale = 0;

		// Play on Awake lol
		//ThemeSong.Play();

		Cursor.visible = false;

		UIManager.Instance.ShowCanvas(0, true);
		UIManager.Instance.ShowCanvas(1, false);
		UIManager.Instance.ShowCanvas(2, false);
	}


	public void StartGame()
	{
		UIManager.Instance.ShowCanvas(0, false);
		UIManager.Instance.ShowCanvas(2, true);

		Time.timeScale = 1;

		_player.PlayerActive = true;

		Vector3 position = Vector3.zero;
		position.x = Random.Range(-45, 45);
		position.y = Random.Range(-45, 45);

		if (position.x < 20f && position.x > -20f)
		{
			position.x *= 4;
		}
		if (position.y < 20f && position.y > -20f)
		{
			position.y *= 4;
		}

		// Spawn a free magic hat
		ItemManager.Instance.SpawnWorldItem(103, position);

		for (int i = 0; i < BossLocs.Length; i++)
		{
			Instantiate(BossPrefab, BossLocs[i], Quaternion.identity);
		}

		_bossesLeft = BossLocs.Length;

		_wave = 0;

		StartNextWave();
	}


	public void PlayerDeath()
	{
		_player.PlayerActive = false;
		UIManager.Instance.ShowCanvas(1, true);
		UIManager.Instance.SetEndMessage("Game Over!");
	}


	public void Restart()
	{
		Time.timeScale = 0;

		UIManager.Instance.ShowCanvas(0, true);
		UIManager.Instance.ShowCanvas(1, false);
		UIManager.Instance.ShowCanvas(2, false);

		SceneManager.LoadScene("ForestScene");
	}

	public void BossKilled()
	{
		_bossesLeft -= 1;

		if (_bossesLeft <= 0)
		{
			PlayerWins();
		}
		else
		{
			StartNextWave();
		}
	}


	public void PlayerWins()
	{
		Time.timeScale = 0;
		UIManager.Instance.ShowCanvas(1, true);
		UIManager.Instance.SetEndMessage("You Win!");
	}

	private void StartNextWave()
	{
		_wave++;

		Vector3 position = Vector3.zero;

		int ents = _wave * 3;
		int blights = _wave * 6;

		for (int i = 0; i < ents; i++)
		{
			position.x = Random.Range(-45, 45);
			position.y = Random.Range(-45, 45);

			if (position.x < 10f && position.x > -10f)
			{
				position.x *= 5;
			}
			if (position.y < 10f && position.y > -10f)
			{
				position.y *= 5;
			}

			MobManager.Instance.SpawnEnemy(102, position);
		}

		for (int i = 0; i < blights; i++)
		{
			position.x = Random.Range(-45, 45);
			position.y = Random.Range(-45, 45);

			if (position.x < 10f && position.x > -10f)
			{
				position.x *= 5;
			}
			if (position.y < 10f && position.y > -10f)
			{
				position.y *= 5;
			}

			MobManager.Instance.SpawnEnemy(103, position);
		}
	}
}
