using System;
using Unity.Entities;

namespace MyTPS
{
    [Serializable]
    public struct InventoryItem
    {
        public ItemOrder order; 
        public Entity prefab;
        public Entity instance;
    }

    [Serializable]
    public struct InventoryItemCollection
    {
        public BlobArray<InventoryItem> items;
    }

    [Serializable]
    public struct PlayerInventory : IComponentData
    {
        public BlobAssetReference<InventoryItemCollection> items;
    }
}
