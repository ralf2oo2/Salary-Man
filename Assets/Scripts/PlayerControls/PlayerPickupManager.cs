using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPickupManager
{
    private static GameObject heldItem;
    public static void SetPickupItem(GameObject item)
    {
        heldItem = item;
    }

    public static GameObject GetPickupItem() 
    {
        return heldItem;
    }

    public static void RemovePickupItem()
    {
        heldItem = null;
    }

    public static bool HasPickupItem()
    {
        return heldItem != null;
    }
}
