using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BasicContainer
{
    #region Events

    public UnityEvent OnInventoryChange = new UnityEvent(); // An event that is when the container is updated

    #endregion

    #region Private

    private Dictionary<int, BasicItem> item = new Dictionary<int, BasicItem>(); // A collection of all items present in the container (main focus slot index)
    public int slotAmount = 20; // The total amount of slots in the container

    #endregion

}

#region HelperFunctions

// Finds the first empty slot and returns its index, or -1 when there is no free slot
public int FindEmptySlot()
{
    for (int i = 0; i < slotAmount; i++)
    {
        if (!items.ContainsKey(i))
        {
            return i;
        }
    }
}

//Finds the first item of a given name, or returns null if there is no such item
public BasicItem FindFirstItemOfName(string itemName)
{
    BasicItem item = null;

    foreach(KeyValuePair<int, BasicItem> entry in items)
    {

    }
}

#endregion
