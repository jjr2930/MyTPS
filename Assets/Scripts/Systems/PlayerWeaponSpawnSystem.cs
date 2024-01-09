using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public partial class PlayerWeaponSpawnSystem : SystemBase
    {
        const string SOCKET_NAME = "G28 Socket";
        EntityQuery playerQuery;

        protected override void OnCreate()
        {
            base.OnCreate();
            RequireForUpdate<FixedTickSystem.Singleton>();

            playerQuery = SystemAPI.QueryBuilder()
                .WithAll<BasicPlayerInputs>()
                .WithAll<BasicPlayer>()
                .WithAll<PlayerInventory>()
                .Build();

            RequireForUpdate<BasicPlayerInputs>();
            RequireForUpdate(playerQuery);
        }

        protected override void OnUpdate()
        {            
            var tick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;
            var entities = playerQuery.ToEntityArray(Allocator.Temp);
            foreach (var entity in entities)
            {
                var basicInput  = SystemAPI.GetComponentRO<BasicPlayerInputs>(entity);
                var basicPlayer = SystemAPI.GetComponentRO<BasicPlayer>(entity);
                var inventory   = EntityManager.GetComponentObject<PlayerInventory>(entity);
                if (EntityManager.HasComponent<AnimatorModelInstanceData>(basicPlayer.ValueRO.ControlledCharacter))
                {
                    var animatorInstance = EntityManager.GetComponentData<AnimatorModelInstanceData>(basicPlayer.ValueRO.ControlledCharacter);
                    var animatorTarnsform = animatorInstance.instance.transform;
                    if (basicInput.ValueRO.primaryPressed.IsSet(tick))
                    {
                        ActivateSelectedItemOnly(inventory, animatorTarnsform, ItemOrder.Primary);
                    }

                    if (basicInput.ValueRO.secondaryPressed.IsSet(tick))
                    {
                        ActivateSelectedItemOnly(inventory, animatorTarnsform, ItemOrder.Secondary);
                    }

                    if (basicInput.ValueRO.handPressed.IsSet(tick))
                    {
                        ActivateSelectedItemOnly(inventory, animatorTarnsform, ItemOrder.Hand);
                    }
                }
            }
        }

        void ActivateSelectedItemOnly(PlayerInventory inventory, Transform characerRoot, ItemOrder order)
        {
            int count = inventory.items.Count;
            for (int i = 0; i<count; ++i)
            {
                var item = inventory.items[i];
                if(null == item.instance)
                {
                    //test by fixed string
                    var found = OOPTransformUtility.FindChildByName(SOCKET_NAME, characerRoot);
                    if (null == found)
                    {
                        throw new InvalidOperationException("can not found socket!");
                    }

                    item.instance = GameObject.Instantiate(item.prefab, found);
                    item.instance.transform.localPosition = Vector3.zero;
                    item.instance.transform.localRotation = Quaternion.identity;
                }

                if (item.order != order)
                    item.instance.gameObject.SetActive(false);
                else
                    item.instance.gameObject.SetActive(true);
            }
        }
    }
}
