using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public bool Active;

    [SerializeField]
    private float _walkSpeed;

    [SerializeField]
    private float _maxHealth;

    [SerializeField]
    private float _attackCoolDown;

    [SerializeField]
    private float _luck;

    [SerializeField]
    private List<AttackHandler> _attacks;

    [SerializeField]
    private Root _rootPrefab;

    [SerializeField]
    private Vector3[] _rootLocs;

    private BoxCollider2D _collider;

    private SpriteRenderer _spriteRenderer;

    private Animator _animator;

    private LayerMask _levelMask;

    private float _timeTillAttack;

    private Vector3 _distToPlayer;

    private BossState _state;

    private float _stageHealth;

    private bool _shielded;

    private int _rootsLeft;

    private enum BossState
	{
        None,
        Passive,
        Rooted,
        Mobile,
	}

    void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        _animator = GetComponent<Animator>();

        _levelMask = 1 << 10;

        Active = true;
    }


	private void Start()
	{
        Initialize();
	}

	// Update is called once per frame
	void Update()
    {
        if (Active)
        {
            if (_timeTillAttack > 0f)
            {
                _timeTillAttack -= Time.deltaTime;
            }


            if (_state == BossState.Passive)
			{
                float distance = (Player.Instance.transform.position - transform.position).magnitude;
                if (distance < 7f)
				{
                    _state = BossState.Rooted;
                    _animator.SetBool("IsPassive", false);
                }
            }

            if (_state == BossState.Mobile)
			{
                Move();

                Attack();
            }
            else if (_state == BossState.Rooted)
			{
                Attack();

                if (_stageHealth <= 0)
				{
                    _state = BossState.Mobile;
                    _attackCoolDown *= 0.5f;
                    _stageHealth = _maxHealth;
                    
				}
            }
            
        }
    }


    private void Move()
    {

        _distToPlayer = Player.Instance.transform.position - transform.position;

        if (_distToPlayer.sqrMagnitude > 1f)
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
        if (_timeTillAttack <= 0f)
        {
            float distance = (Player.Instance.transform.position - transform.position).magnitude;
            
            if (distance <= 20f)
            {
                Vector3 direction = (Player.Instance.transform.position - transform.position).normalized;

                int attack = Random.Range(0, _attacks.Count);
                _attacks[attack].Attack(transform.position, direction, false);
            }

            _animator.SetTrigger("AttackTrigger");

            _timeTillAttack = _attackCoolDown;
        }
    }



    public void Initialize()
    {
        _state = BossState.Passive;
        _stageHealth = _maxHealth;
        _shielded = true;
        _animator.SetBool("IsPassive", true);
        _rootsLeft = _rootLocs.Length;

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


        for (int i = 0; i < _rootLocs.Length; i++)
		{
            Vector3 position = transform.position + _rootLocs[i];
            Root root = Instantiate(_rootPrefab, position, Quaternion.identity);
            root.ParentBoss = this;
        }
    }


    public void TakeDamage(float damage)
    {
        if (_shielded)
		{
            return;
		}

        _stageHealth -= damage;

        if (_stageHealth <= 0f && _state == BossState.Mobile)
        {
            _animator.SetTrigger("DeathTrigger");

            AudioManager.Instance.PlaySound(3);

            Active = false;
            // Destroy(gameObject);
        }
        else
        {
            AudioManager.Instance.PlaySound(2);

            _animator.SetTrigger("DamageTrigger");
        }
    }


    public void RootDied()
	{
        _rootsLeft -= 1;

        if (_rootsLeft <= 0)
		{
            _shielded = false;
		}
	}
    private void OnDeathExit(string stateName)
    {
        // Debug.Log("Death exit on " + stateName);

        ItemManager.Instance.SpawnRandomItem(_luck, transform.position);

        GameManager.Instance.BossKilled();

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
