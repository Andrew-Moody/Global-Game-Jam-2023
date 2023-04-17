using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemTest : MonoBehaviour
{
	public int id;

	private void Start()
	{
		ItemManager.Instance.SpawnWorldItem(id, transform.position);
	}
}
