using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class ItemDetect : Detecting
{
    List<Item> items = new();
    public List<Item> GetItems()
    {
        Collider[] value = Detect();

        items.Clear();
        foreach (Collider item in value)
        {
            if(item.TryGetComponent(out Item _item))
            {
                items.Add(_item);
            }
        }

        return items;
    }

}
