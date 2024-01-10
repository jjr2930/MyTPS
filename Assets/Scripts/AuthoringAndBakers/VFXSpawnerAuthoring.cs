using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{

    public class VFXSpawnerAuthoring : MonoBehaviour
    {
        [Serializable]
        public class VFXInfo
        {
            public VFXType type;
            public PoolType poolType;
        }

        public List<VFXInfo> infos = new List<VFXInfo>();

        public class Baker : Baker<VFXSpawnerAuthoring>
        {
            public override void Bake(VFXSpawnerAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var builder = new BlobBuilder(Allocator.Temp);
                ref VFXPool vfxPool = ref builder.ConstructRoot<VFXPool>();
                BlobBuilderArray<MyTPS.VFXInfo> vfxArrayBuilder = builder.Allocate(
                        ref vfxPool.vfxs,
                        authoring.infos.Count
                    );

                for(int i = 0; i<authoring.infos.Count; i++)
                {
                    vfxArrayBuilder[i] = new MyTPS.VFXInfo()
                    {
                        type = authoring.infos[i].type,
                        poolType = authoring.infos[i].poolType
                    };
                }
                                
                var vfxSpawner = new VFXSpawner();
                vfxSpawner.vfxPool = builder.CreateBlobAssetReference<VFXPool>(Allocator.Persistent);
                builder.Dispose();

                AddComponent(entity, vfxSpawner);
            }
        }
    }
}
