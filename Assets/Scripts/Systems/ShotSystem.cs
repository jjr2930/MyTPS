
using Unity.Entities;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Transforms;
using UnityEngine;

namespace MyTPS
{

    public partial struct ShotSystem : ISystem
    {
        public void OnCreate(ref SystemState state)
        {            
            state.RequireForUpdate<BasicPlayerInputs>();
            state.RequireForUpdate<BasicPlayer>();
            state.RequireForUpdate<PhysicsWorldSingleton>();
        }

        public void OnUpdate(ref SystemState state)
        {
            var elapsedTime = SystemAPI.Time.ElapsedTime;

            var physicsWorldSinglton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
            var physicsWorld = physicsWorldSinglton.CollisionWorld;
            var tick = SystemAPI.GetSingleton<FixedTickSystem.Singleton>().Tick;

            foreach (var (basicInput, basicPlayer) 
                in SystemAPI.Query<RefRW<BasicPlayerInputs>,RefRW<BasicPlayer>>())
            {
                if (basicInput.ValueRO.firePressed)
                {
                    var lastTime = basicPlayer.ValueRO.lastTime;
                    var interval = basicPlayer.ValueRO.fireInterval;

                    if (elapsedTime - lastTime >= interval)
                    {
                        basicPlayer.ValueRW.lastTime = elapsedTime;
                        basicPlayer.ValueRW.lastShotTick = tick;
                        var cameraWorld = SystemAPI.GetComponentRO<LocalToWorld>(basicPlayer.ValueRO.ControlledCamera);

                        Debug.DrawRay(cameraWorld.ValueRO.Position, cameraWorld.ValueRO.Forward);

                        var raycastInput = new RaycastInput
                        {
                            Start = cameraWorld.ValueRO.Position,
                            End = cameraWorld.ValueRO.Position + cameraWorld.ValueRO.Forward * 1000f,
                            //Start = Camera.main.transform.position,
                            //End = Camera.main.transform.position + Camera.main.transform.forward * 1000f,
                            Filter = new CollisionFilter
                            {
                                GroupIndex = 0
                                ,BelongsTo = 1u << 0
                                ,CollidesWith = 1u << 1
                            }
                        };

;                       //var collector = new ClosestHitCollector<ColliderCastHit>(1000f);
                        Unity.Physics.RaycastHit hit;
                        if(physicsWorld.CastRay(raycastInput, out hit))
                        {
                            Debug.Log("Hit! : " + hit.Position);
                            AimDebugCube.Instance.transform.position = hit.Position;
                        }
                    }
                }
            }
        }
    }
}