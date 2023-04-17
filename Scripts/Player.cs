using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    public static Player Instance;

    public bool PlayerActive;

    [SerializeField]
    private float _walkSpeed;

    [SerializeField]
    private float _acceleration;

    [SerializeField]
    private LayerMask _levelMask;

    [SerializeField]
    private LayerMask _entityMask;

    [SerializeField]
    private Inventory _inventory;

    [SerializeField]
    private Stats _stats;

    private Vector2 _velocity;

    private BoxCollider2D _collider;

    private Animator _animator;

    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private float _maxHealth;

    private float _health;

    [SerializeField]
    private float _maxMana;

    private float _mana;

    private float _timeSinceRegen;

    private float _damageReduction;

    private bool _weaponEquiped;

    [SerializeField]
    private AttackHandler _mainAttack;

    [SerializeField]
    private AttackHandler _secAttack;

    [SerializeField]
    private Weapon _weapon;

    [SerializeField]
    private int _starterItem;


    void Awake()
    {
        if (Instance == null)
		{
            Instance = this;
		}

        _collider = GetComponent<BoxCollider2D>();

        _animator = GetComponent<Animator>();

        _spriteRenderer = GetComponent<SpriteRenderer>();

        PlayerActive = true;
    }


	private void Start()
	{
        Initialize();
    }


	// Update is called once per frame
	void Update()
    {
        if (PlayerActive)
		{
            _timeSinceRegen -= Time.deltaTime;
            if (_timeSinceRegen <= 0f)
			{
                _health += 5;
                _mana += 5;

                if (_health > _maxHealth)
                {
                    _health = _maxHealth;
                }

                if (_mana > _maxMana)
				{
                    _mana = _maxMana;
				}

                _stats.UpdateHealth(_health / _maxHealth);
                _stats.UpdateMana(_mana / _maxMana);

                _timeSinceRegen = 5;
			}

            // Handle movement first (otherwise might allow improper interactions)
            Move();

            // Handle entity interaction
            Interact();
        }
    }


    public void Initialize()
	{
        _health = _maxHealth;
        _mana = _maxMana;
        _stats.UpdateHealth(_health / _maxHealth);
        _stats.UpdateHealth(_mana / _maxMana);

        _inventory.InitializeSlots();

        ItemData starterItem = ItemManager.Instance.SpawnItem(_starterItem);

        _inventory.TryTakeItem(ref starterItem);
    }



	private void Move()
	{
        float horiz = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        Vector3 target = new Vector3(horiz, vert, 0f).normalized;

        if (horiz < 0f)
		{
            _spriteRenderer.flipX = true;
            _weapon.FlipX(true);
        }
        else if (horiz > 0f)
		{
            _spriteRenderer.flipX = false;
            _weapon.FlipX(false);
        }
        
        if (horiz == 0f && vert == 0f)
		{
            _animator.SetBool("IsMoving", false);
		}
		else
		{
            _animator.SetBool("IsMoving", true);
        }


        transform.Translate(target * Time.deltaTime * _walkSpeed);


        // This only works if "Auto Sync Transforms" is set to true in Physics 2D settings
        // This makes updates to position immediate rather than at the end of the frame I believe
        Collider2D[] levelhits = Physics2D.OverlapBoxAll(transform.position, _collider.size, 0f, _levelMask);

        foreach (Collider2D hit in levelhits)
        {
            ColliderDistance2D distance = hit.Distance(_collider);

            // Looks like isOverlapped does update when translated immediately (as long as Auto Sync is true)
            if (distance.isOverlapped)
			{
                transform.Translate(distance.pointA - distance.pointB);
			}
        }
    }


    private void Interact()
	{
        if (!EventSystem.current.IsPointerOverGameObject() && _weaponEquiped)
		{
            bool mainAction = Input.GetButtonDown("Fire1");
            bool secondaryAction = Input.GetButtonDown("Fire2");

            if (mainAction)
            {
                Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                worldPoint.z = 0;

                Vector3 direction = (worldPoint - transform.position).normalized;

                _mainAttack.Attack(transform.position, direction, true);

                _weapon.SwingWeapon();

                AudioManager.Instance.PlaySound(5);

                Debug.Log("Fire1");
            }

            if (secondaryAction)
            {
                if (_mana >= _secAttack.ManaCost)
				{
                    _mana -= _secAttack.ManaCost;

                    _stats.UpdateMana(_mana / _maxMana);

                    Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    worldPoint.z = 0;

                    Vector3 direction = (worldPoint - transform.position).normalized;

                    _secAttack.Attack(transform.position, direction, true);

                    _weapon.SwingWeapon();

                    AudioManager.Instance.PlaySound(4);

                    Debug.Log("Fire2");
                }
                
            }
        }


        Collider2D[] entityhits = Physics2D.OverlapBoxAll(transform.position, _collider.size, 0f, _entityMask);

        foreach (Collider2D entity in entityhits)
        {
            if (entity.CompareTag("Enemy"))
			{
                Enemy enemy = entity.GetComponent<Enemy>();
                if (enemy != null)
				{
                    if (enemy.CheckAttack())
					{
                        TakeDamage(enemy.GetDamage());
					}
				}

			}

            if (entity.CompareTag("Item"))
			{
                Debug.Log("Hit Item");

                WorldItem worldItem = entity.GetComponent<WorldItem>();
                if (worldItem != null)
				{
                    ItemData itemData = worldItem.GetItemData();

                    if (_inventory.TryTakeItem(ref itemData))
					{
                        // Item fully picked up, can destroy worlditem
                        Destroy(worldItem.gameObject);

                        AudioManager.Instance.PlaySound(1);
                    }
					else
					{
                        // Item at least partially not picked up
                        worldItem.SetItemData(itemData);
					}
				}
			}

        }
    }


    public void TakeDamage(float damage)
	{
        if (!PlayerActive)
            return;

        damage = damage * (1 - _damageReduction);

        _health -= damage;

        AudioManager.Instance.PlaySound(6);

        Debug.Log($"Took {damage} Damage!");

        if (_health <= 0f)
        {
            _health = 0f;
            _stats.UpdateHealth(_health / _maxHealth);

            _animator.SetTrigger("DeathTrigger");

            GameManager.Instance.PlayerDeath();
            return;
        }

        _stats.UpdateHealth(_health / _maxHealth);
    }


    public void EquipItem(ItemData item)
	{
        if (item.ID == 0 && item.Quantity == 0)
		{
            // Dequip
            _weapon.gameObject.SetActive(false);
            _weaponEquiped = false;
            return;
        }

        if (!item.Stats.IsWeapon)
		{
            _damageReduction = item.Stats.DamageReduction;
		}
		else
		{
            _weapon.gameObject.SetActive(true);
            _weaponEquiped = true;
            _weapon.SetSprite(item.Sprite);
            _mainAttack.ProjectileSO = item.Stats.ProjSO_1;
            _mainAttack.Angle = item.Stats.Angle_1;
            _mainAttack.NumberProjectiles = item.Stats.NumProj_1;
            _mainAttack.ManaCost = item.Stats.ManaCost_1;

            _secAttack.ProjectileSO = item.Stats.ProjSO_2;
            _secAttack.Angle = item.Stats.Angle_2;
            _secAttack.NumberProjectiles = item.Stats.NumProj_2;
            _secAttack.ManaCost = item.Stats.ManaCost_2;
        }
	}


    public void ConsumeItem(ItemData item)
	{
        if (item.Stats.IsConsumable)
		{
            _health += item.Stats.HealthRestore;
            _mana += item.Stats.ManaRestore;

            if (_health > _maxHealth)
			{
                _health = _maxHealth;
			}
            
            if (_mana > _maxMana)
			{
                _mana = _maxMana;
			}

            _stats.UpdateHealth(_health / _maxHealth);
            _stats.UpdateMana(_mana / _maxMana);
        }
	}
}
