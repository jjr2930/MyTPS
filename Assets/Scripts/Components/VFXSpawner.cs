using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.VFX;
using static MyTPS.VFXSpawner;

namespace MyTPS
{
    public enum VFXType
    {
        MulzzleFire,
    }

    public struct VFXInfo
    {
        public VFXType type;
        public PoolType poolType;
    }

    public struct VFXPool
    {
        public BlobArray<VFXInfo> vfxs;
    }


    public struct VFXSpawner : IComponentData
    {
        public BlobAssetReference<VFXPool> vfxPool;
    }
}
