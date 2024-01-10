using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JLib.ObjectPool;
using UnityEngine.Pool;
using System;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace MyTPS
{
    public enum PoolType
    {
        MuzzleFlame,
    }
    public class MyTPSObjectPool : DefaultObjectPool<PoolType>
    {
    }

    public static class MyTPSObjectPoolExtension
    {
        public static void PopOne(this DefaultObjectPool<PoolType> self, PoolType type, Vector3 position, Quaternion rotation, Vector3 localScale, Transform parent, bool isLocalSpace = true)
        {
            var one = self.PopOne(type);
            if (null != parent)
            {
                one.transform.SetParent(parent);
            }

            one.transform.localScale = localScale;

            if (isLocalSpace)
            {
                one.transform.SetLocalPositionAndRotation(position, rotation);
            }
            else
            {
                one.transform.SetPositionAndRotation(position, rotation);
            }
        }
    }
}