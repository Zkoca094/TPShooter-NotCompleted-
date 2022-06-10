﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

[System.Serializable]
public class Inventory
{
    public InventorySlot[] Slots;

    public Inventory(int max_Item)
    {
        Slots = new InventorySlot[max_Item];
    }
    public void Clear()
    {
        for (int i = 0; i < Slots.Length; i++)
        {
            Slots[i].item = new Item();
            Slots[i].amount = 0;
        }
    }

    public bool ContainsItem(ItemObject itemObject)
    {
        return Array.Find(Slots, i => i.item.Id == itemObject.data.Id) != null;
    }


    public bool ContainsItem(int id)
    {
        return Slots.FirstOrDefault(i => i.item.Id == id) != null;
    }
}