using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "ScriptableObjects/Item")]
public class ItemSO : ScriptableObject
{
    public int ID;

    public string ItemName;

    public int StackLimit;

    public int EquipSlot;

    public Sprite Sprite;

    public StatsSO Stats;
}
