using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool Active;

    [SerializeField]
    private float _walkSpeed;

    [SerializeField]
    private float _acceleration;

    [SerializeField]
    private Projectile _projPrefab;

    [SerializeField]
    private LayerMask _levelMask;

    private BoxCollider2D _collider;

    private SpriteRenderer _spriteRenderer;

    private Animator _animator;

    private MobData _mobData;

    private float _timeTillAttack;

    private Vector3 _distToPlayer;

    private float _stunTime;

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _animator = GetComponent<Animator>();

        _levelMask = 1 << 10;

        Active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
		{
            if (_stunTime > 0f)
			{
                _stunTime -= Time.deltaTime;
                return;
			}

            if (_timeTillAttack > 0f)
            {
                _timeTillAttack -= Time.deltaTime;
            }

            Move();

            Attack();
        }
    }


    private void Move()
    {

        _distToPlayer = Player.Instance.transform.position - transform.position;

        if (_distToPlayer.sqrMagnitude > _mobData.MeleeRange)
		{
            Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;

            transform.Translate(direction * Time.deltaTime * _walkSpeed);

            Collider2D[] levelhits = Physics2D.OverlapBoxAll(transform.position, _collider.size, 0f, _levelMask);

            foreach (Collider2D hit in levelhits)
            {
                ColliderDistance2D distance = hit.Distance(_collider);

                if (distance.isOverlapped)
                {
                    transform.Translate(distance.pointA - distance.pointB);
                }
            }

            if (direction.x < 0f)
            {
                _spriteRenderer.flipX = true;
            }
            else if (direction.x > 0f)
            {
                _spriteRenderer.flipX = false;
            }

            _animator.SetBool("IsMoving", true);
        }
		else
		{
            _animator.SetBool("IsMoving", false);
		}
    }


    private void Attack()
	{
        if (_timeTillAttack <= 0f && _mobData.Hostile)
        {
            float distance = (Player.Instance.transform.position - transform.position).magnitude;
            if (distance <= _mobData.MeleeRange)
			{
                // Melee Attack;
                Player.Instance.TakeDamage(_mobData.AttackDamage);
			}
			else if (_mobData.ProjectileSO != null && distance <= 10f)
			{
                Vector3 velocity = _mobData.ProjectileSO.Speed * (Player.Instance.transform.position - transform.position).normalized;

                MobManager.Instance.SpawnProjectile(_mobData.ProjectileSO, transform.position, velocity, false);
            }

            _animator.SetTrigger("AttackTrigger");

            _timeTillAttack = _mobData.AttackCoolDown;
        }
    }


    public bool CheckAttack()
	{
        return false;
	}


    public float GetDamage()
	{
        return _mobData.AttackDamage;
	}


    public void SetMobData(MobData data)
	{
        _mobData = data;

        _spriteRenderer.sprite = data.Sprite;

        _collider.size = new Vector2(data.Size, data.Size);

        _walkSpeed = data.Speed;

        _animator.runtimeAnimatorController = data.AnimController;

        AnimatorState[] states = _animator.GetBehaviours<AnimatorState>();
        foreach (AnimatorState state in states)
		{
            if (state.StateName == "Death")
			{
                state.StateExitEvent += OnDeathExit;
			}
            else if (state.StateName == "Damage")
			{
                state.StateExitEvent += OnDamageExit;
            }
            else if (state.StateName == "Attack")
            {
                state.StateExitEvent += OnAttackExit;
            }
        }
	}


    public void TakeDamage(float damage)
	{
        _mobData.Health -= damage;

        StopCoroutine(ShowDamage());
        StartCoroutine(ShowDamage());

        if (_mobData.Health <= 0f)
		{
            _animator.SetTrigger("DeathTrigger");

            AudioManager.Instance.PlaySound(3);

            Active = false;
            // Destroy(gameObject);
		}
		else
		{
            // Apply "stun";
            _stunTime = _mobData.AttackCoolDown;
            _timeTillAttack = _mobData.AttackCoolDown;

            AudioManager.Instance.PlaySound(2);

            _animator.SetTrigger("DamageTrigger");
        }
	}

    private IEnumerator ShowDamage()
    {
        Color color = Color.white;

        for (float bg = 1f; bg >= 0; bg -= (5 * Time.deltaTime))
        {
            color.b = bg;
            color.g = bg;
            _spriteRenderer.color = color;
            yield return null;
        }

        for (float bg = 0f; bg <= 1f; bg += (5 * Time.deltaTime))
        {
            Debug.Log("test");
            color.b = bg;
            color.g = bg;
            _spriteRenderer.color = color;
            yield return null;
        }

        _spriteRenderer.color = Color.white;
    }

    private void OnDeathExit(string stateName)
	{
        // Debug.Log("Death exit on " + stateName);

        ItemManager.Instance.SpawnRandomItem(_mobData.Luck, transform.position);

        Destroy(gameObject);
	}


    private void OnDamageExit(string stateName)
    {
        // Debug.Log("Damage exit on " + stateName);
    }


    private void OnAttackExit(string stateName)
    {
        // Debug.Log("Attack exit on " + stateName);
    }
}
