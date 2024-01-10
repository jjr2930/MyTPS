using JLib.ObjectPool;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace MyTPS
{
    /// <summary>
    /// https://forum.unity.com/threads/object-pooling-in-dots.886789/
    /// do not make any pool
    /// </summary>
    public partial class VFXSpawnSystem : SystemBase
    {
        SystemHandle ecbSystem;
        protected override void OnCreate()
        {
            base.OnCreate();

            RequireForUpdate<VFXSpawner>();
            RequireForUpdate<FixedTickSystem.Singleton>();
        }

        protected override void OnUpdate()
        {
            var tick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;
            var vfxSpawner = SystemAPI.GetSingleton<VFXSpawner>();


            //Entities.WithDeferredPlaybackSystem<EndSimulationEntityCommandBufferSystem>()
            //    .ForEach(
            //    (Entity entity, EntityCommandBuffer ecb, in BasicPlayerInputs input, in BasicPlayer player) =>
            //    {
            //        if(tick != player.lastShotTick)
            //        {
            //            return;
            //        }

            //         var characterEntity = player.ValueRW.ControlledCharacter;
            //    var animatorInstanceData = EntityManager.GetComponentObject<AnimatorModelInstanceData>(characterEntity);
            //    var animatorInstance = animatorInstanceData.instance;
            //    var muzzleFlamePoint = animatorInstance.GetComponentInChildren<MuzzleFlamePoint>(false);
            //    if (null == muzzleFlamePoint)
            //        return;
            //    }).ScheduleParallel();


            foreach (var (input, player)
                in SystemAPI.Query<RefRW<BasicPlayerInputs>, RefRW<BasicPlayer>>())
            {
                if (tick != player.ValueRO.lastShotTick)
                    continue;

                var characterEntity = player.ValueRW.ControlledCharacter;
                var animatorInstanceData = EntityManager.GetComponentObject<AnimatorModelInstanceData>(characterEntity);
                var animatorInstance = animatorInstanceData.instance;
                var muzzleFlamePoint = animatorInstance.GetComponentInChildren<MuzzleFlamePoint>(false);
                if (null == muzzleFlamePoint)
                    return;

                for (int i = 0; i < vfxSpawner.vfxPool.Value.vfxs.Length; ++i)
                {
                    var poolType = vfxSpawner.vfxPool.Value.vfxs[i].poolType;
                    var vfxType = vfxSpawner.vfxPool.Value.vfxs[i].type;
                    if (vfxType == muzzleFlamePoint.vfxType)
                    {
                        MyTPSObjectPool.Instance.PopOne(poolType 
                            , Vector3.zero
                            , Quaternion.identity
                            , Vector3.one
                            , muzzleFlamePoint.transform);
                    }
                }
            }
        }
    }
}
