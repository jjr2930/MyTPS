using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public partial class PlayerWeaponSpawnSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            foreach(var (basicInput, basicPlayer, playerInventory)
                in SystemAPI.Query<RefRW<BasicPlayerInputs>, RefRW<BasicPlayer>, RefRW<PlayerInventory>>())
            {

            }
        }
    }
}
