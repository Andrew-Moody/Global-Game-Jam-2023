using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobManager : MonoBehaviour
{
    public static MobManager Instance;

	[SerializeField]
	private Enemy _enemyPrefab;

    [SerializeField]
    private List<MobSO> _mobSOs;

	[SerializeField]
	private Projectile _projectilePrefab;

    private Dictionary<int, MobSO> _mobDict;
    
    void Awake()
    {
        if (Instance == null)
		{
            Instance = this;
		}

		_mobDict = new Dictionary<int, MobSO>();

        for (int i = 0; i < _mobSOs.Count; i++)
		{
			if (_mobDict.ContainsKey(_mobSOs[i].ID))
			{
				Debug.Log($"Mob ID {_mobSOs[i].ID} already exists");
			}
			else
			{
				_mobDict.Add(_mobSOs[i].ID, _mobSOs[i]);
			}
		}
    }


    public MobData CreateMobData(int id)
	{
		if (_mobDict.TryGetValue(id, out MobSO mobSO))
		{
			return new MobData(mobSO);
		}
		else
		{
			Debug.Log($"No Mob exists for item id: {id}");
			return new MobData();
		}
	}


	public Enemy SpawnEnemy(int id, Vector3 position)
	{
		Enemy enemy = Instantiate(_enemyPrefab, position, Quaternion.identity);
		if (enemy != null)
		{
			enemy.SetMobData(CreateMobData(id));
		}

		return enemy;
	}


	public Projectile SpawnProjectile(ProjectileSO projectileSO, Vector3 startPos, Vector3 velocity, bool isPlayer = false)
	{
		Projectile projectile = Instantiate(_projectilePrefab, startPos, Quaternion.identity);

		if (projectile == null)
		{
			return null;
		}

		projectile.Initialize(projectileSO);
		projectile.Launch(startPos, velocity, isPlayer);

		return projectile;
	}
}
