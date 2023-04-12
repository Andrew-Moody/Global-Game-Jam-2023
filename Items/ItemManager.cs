using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    [SerializeField]
    private List<ItemSO> _itemSOs;

	[SerializeField]
	private WorldItem _worldItemPrefab;

	[SerializeField]
	private List<int> _t1Drops;

	[SerializeField]
	private List<int> _t2Drops;

	[SerializeField]
	private List<int> _t3Drops;


	private Dictionary<int, ItemSO> _itemDict;


    void Awake()
    {
        if (Instance == null)
		{
            Instance = this;
		}

		_itemDict = new Dictionary<int, ItemSO>();

		for (int i = 0; i < _itemSOs.Count; i++)
		{
			if (_itemDict.ContainsKey(_itemSOs[i].ID))
			{
				Debug.Log($"Item ID {_itemSOs[i].ID} already exists");
			}
			else
			{
				_itemDict.Add(_itemSOs[i].ID, _itemSOs[i]);
			}
		}
	}


	private void Start()
	{
		
	}

	public ItemData SpawnItem(int id)
	{
		if (_itemDict.TryGetValue(id, out ItemSO itemSO))
		{
			return new ItemData(itemSO);
		}
		else
		{
			Debug.Log($"No item exists for item id: {id}");
			return new ItemData();
		}

	}


	public WorldItem SpawnWorldItem(ItemData itemData, Vector3 position)
	{
		WorldItem worldItem = Instantiate(_worldItemPrefab, position, Quaternion.identity);

		worldItem.SetItemData(itemData);

		return worldItem;
	}


	public WorldItem SpawnWorldItem(int id, Vector3 position)
	{
		return SpawnWorldItem(SpawnItem(id), position);
	}


	public WorldItem SpawnRandomItem(float luck, Vector3 position)
	{
		int id = 0;

		int roll = Random.Range(0, 1000);

		roll = (int)((float)roll / luck);

		Debug.Log("Luck Roll: " + roll);

		if (roll < 10)
		{
			// T3 table
			id = _t3Drops[Random.Range(0, _t3Drops.Count)];

		}
		else if (roll < 50)
		{
			// T2 table
			id = _t2Drops[Random.Range(0, _t2Drops.Count)];
		}
		else if (roll < 500)
		{
			// T1 table
			id = _t1Drops[Random.Range(0, _t1Drops.Count)];
		}

		if (id == 0)
		{
			// No Drops
			return null;
		}



		return SpawnWorldItem(SpawnItem(id), position);
	}
}
