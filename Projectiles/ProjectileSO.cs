using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewProjectile", menuName = "ScriptableObjects/Projectile")]
public class ProjectileSO : ScriptableObject
{
    public int ID;

    public float Radius;

    public float Speed;

    public float MaxDistance;

    public float Damage;

    public Sprite Sprite;

    public Color Color;
}
