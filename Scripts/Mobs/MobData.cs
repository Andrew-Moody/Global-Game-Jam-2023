using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct MobData
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


    public MobData(MobSO mobSO)
	{
        ID = mobSO.ID;

        MobName = mobSO.MobName;

        Sprite = mobSO.Sprite;

        AnimController = mobSO.AnimController;

        Size = mobSO.Size;

        Speed = mobSO.Speed;

        Hostile = mobSO.Hostile;

        Luck = mobSO.Luck;

        Health = mobSO.Health;

        MaxHealth = mobSO.MaxHealth;

        AttackCoolDown = mobSO.AttackCoolDown;

        AttackDamage = mobSO.AttackDamage;

        MeleeRange = mobSO.MeleeRange;

        ProjectileSO = mobSO.ProjectileSO;
    }
}
