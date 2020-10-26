using System;
using System.Collections.Generic;

public class ItemsContainer<T, D> where D : class
{
    public uint Id { get; set; }
    public T InventoryId { get; set; }
    public Dictionary<int, D> Items { get; private set; } = new Dictionary<int, D>();

    public event Action<ItemsContainer<T, D>> InventoryChanged = delegate { };
    public int InventorySpace = 16;

    public ItemsContainer(T containerId, uint id)
    {
        this.InventoryId = containerId;
        Id = id;
    }

    public void Refresh()
    {
        InventoryChanged(this);
    }

    public int GetFreeSlot()
    {
        for (int i = 0; i < InventorySpace; i++)
        {
            if (!Items.ContainsKey(i))
            {
                return i;
            }
        }

        return -1;
    }

    public bool GetItem(int slot, out D item)
    {
        if (Items.ContainsKey(slot))
        {
            item = Items[slot];
        }
        else
        {
            item = null;
        }

        return Items.ContainsKey(slot);
    }

    public void AddItem(int slot, D item, bool ignoreEvents = false)
    {
        Console.WriteLine("Add item to slot: " + slot + " / " + InventoryId);
        if (!Items.ContainsKey(slot))
        {
            Items[slot] = item;
            InventoryChanged(this);
        }
    }

    public void Clear()
    {
        Items.Clear();
    }

    public void AddItemToFreeSlot(D item, bool ignoreEvents = false)
    {
        int slot = GetFreeSlot();
        if (slot != -1)
        {
            if (!Items.ContainsKey(slot))
            {
                Items[slot] = item;
                InventoryChanged(this);
            }
        }
    }

    public void RemoveItem(int slot, bool ignoreEvents = false)
    {
        if (Items.ContainsKey(slot))
        {
            Items.Remove(slot);
            if (!ignoreEvents)
            {
                InventoryChanged(this);
            }
        }
    }
}