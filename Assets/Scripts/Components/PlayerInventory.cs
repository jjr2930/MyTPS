using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    [Serializable]
    public class InventoryItem : IDisposable
    {
        public ItemOrder order;
        public string socketName;
        public GameObject prefab;
        public GameObject instance;
        public float fireInterval;

        void IDisposable.Dispose()
        {
            UnityEngine.Object.DestroyImmediate(instance);
        }
    }

    [Serializable]
    public class PlayerInventory : IComponentData, IDisposable
    {
        public List<InventoryItem> items = new List<InventoryItem>();
        public InventoryItem GetItemByOrder(ItemOrder order)
        {
            for (int i = 0; i < items.Count; ++i)
            {
                if (items[i].order == order)
                    return items[i];
            }

            return null;
        }

        void IDisposable.Dispose()
        {
            items.Clear();
        }
    }
}
