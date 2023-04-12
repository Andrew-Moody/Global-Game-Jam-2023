using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Sprite Sprite { get { return _spriteRenderer.sprite; } set { _spriteRenderer.sprite = value; } }

	public Color Color { set { _spriteRenderer.color = value; } }

	public bool Active;

	public bool FromPlayer;

	public float Speed;

	public float Damage;

	private Vector3 _startPos;

	private LayerMask _playerMask;

	private LayerMask _entityMask;

	private SpriteRenderer _spriteRenderer;

	private Vector3 _velocity;

	private float _maxDistance;

	private float _radius;

	private void Awake()
	{
		_spriteRenderer = GetComponent<SpriteRenderer>();

		_playerMask = 1 << 9;
		_entityMask = 1 << 11;
	}

	// Update is called once per frame
	void Update()
    {
        if (Active)
		{
			if ((transform.position - _startPos).magnitude > _maxDistance)
			{
				Destroy(gameObject);
			}

			transform.Translate(_velocity * Time.deltaTime);

			if (FromPlayer)
			{
				Collider2D hit = Physics2D.OverlapCircle(transform.position, _radius, _entityMask);

				if (hit != null)
				{
					Enemy enemy = hit.gameObject.GetComponent<Enemy>();

					if (enemy != null)
					{
						enemy.TakeDamage(Damage);
						Destroy(gameObject);
						return;
					}

					Root root = hit.gameObject.GetComponent<Root>();
					if (root != null)
					{
						root.TakeDamage(Damage);
						Destroy(gameObject);
						return;
					}

					Boss boss = hit.gameObject.GetComponent<Boss>();
					if (boss != null)
					{
						boss.TakeDamage(Damage);
						Destroy(gameObject);
						return;
					}
				}
			}
			else
			{
				Collider2D hit = Physics2D.OverlapCircle(transform.position, _radius, _playerMask);

				if (hit != null)
				{
					Player player = hit.gameObject.GetComponent<Player>();

					if (player != null)
					{
						player.TakeDamage(Damage);
						Destroy(gameObject);
					}
				}
			}
			
		}
    }


	public void Initialize(ProjectileSO projSO)
	{
		_maxDistance = projSO.MaxDistance;

		_radius = projSO.Radius;

		Damage = projSO.Damage;

		Sprite = projSO.Sprite;

		Color = projSO.Color;
	}


	public void Launch(Vector3 position, Vector3 velocity, bool isPlayer = false)
	{
		Active = true;
		FromPlayer = isPlayer;

		_startPos = position;
		_velocity = velocity;

	}
}
