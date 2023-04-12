using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMob", menuName = "ScriptableObjects/Mob")]
public class MobSO : ScriptableObject
{
    public int ID;

    public string MobName;

    public Sprite Sprite;

    public RuntimeAnimatorController AnimController;

    public float Size;

    public float Speed;

    public bool Hostile;

    public float Luck;

    public float Health;

    public float MaxHealth;

    public float AttackCoolDown;

    public float AttackDamage;

    public float MeleeRange;

    public ProjectileSO ProjectileSO;
}
