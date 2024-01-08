using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public class PlayerInventoryAuthoring : MonoBehaviour
    {
        [Serializable]
        public class InventoryWeaponItem
        {
            public ItemOrder order;
            public GameObject prefab;
        }

        public InventoryWeaponItem[] weapons;

        public class Baker : Baker<PlayerInventoryAuthoring>
        {
            public override void Bake(PlayerInventoryAuthoring authoring)
            {
                var playerEntity = GetEntity(TransformUsageFlags.Dynamic);
                var blobBuilder = new BlobBuilder(Allocator.Temp);
                ref InventoryItemCollection itemCollection = ref blobBuilder.ConstructRoot<InventoryItemCollection>();
                BlobBuilderArray<InventoryItem> arrayBuilder = blobBuilder.Allocate(
                        ref itemCollection.items,
                        authoring.weapons.Length
                    );

                if(null != authoring.weapons && authoring.weapons.Length > 0)
                {
                    for (int i = 0; i < authoring.weapons.Length; i++)
                    {
                        arrayBuilder[i] = new InventoryItem
                        {
                            order = authoring.weapons[i].order,
                            prefab = GetEntity(authoring.weapons[i].prefab, TransformUsageFlags.Dynamic)
                        };
                    }
                }

                var result = blobBuilder.CreateBlobAssetReference<InventoryItemCollection>(Allocator.Persistent);                

                AddComponent(playerEntity, new PlayerInventory()
                {
                    items = result
                });

                blobBuilder.Dispose();
            }
        }
    }
}
