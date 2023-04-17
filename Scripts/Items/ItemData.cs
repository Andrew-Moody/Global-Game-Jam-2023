using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ItemData
{
    public int ID;

    public int Quantity;

    public string ItemName;

    public int StackLimit;

    public int EquipSlot;

    public Sprite Sprite;

    public StatsSO Stats;


    public ItemData(ItemSO itemSO, int quantity = 1)
	{
        ID = itemSO.ID;
        ItemName = itemSO.ItemName;
        StackLimit = itemSO.StackLimit;
        EquipSlot = itemSO.EquipSlot;
        Sprite = itemSO.Sprite;
        Stats = itemSO.Stats;
        Quantity = quantity;
	}
}
