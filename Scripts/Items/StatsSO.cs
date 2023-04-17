using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStats", menuName = "ScriptableObjects/ItemStats")]
public class StatsSO : ScriptableObject
{
    public bool IsWeapon;

    public bool IsConsumable;

    public float HealthRestore;

    public float ManaRestore;

    public float DamageReduction;

    public ProjectileSO ProjSO_1;
    public float ManaCost_1;
    public float Angle_1;
    public int NumProj_1;

    public ProjectileSO ProjSO_2;
    public float ManaCost_2;
    public float Angle_2;
    public int NumProj_2;
}
