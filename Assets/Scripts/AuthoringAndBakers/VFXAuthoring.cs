using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace MyTPS
{
    public class VFXAuthoring : MonoBehaviour
    {
        public class Baker : Baker<VFXAuthoring>
        {
            public override void Bake(VFXAuthoring authoring)
            {
                //GetEntity()
            }
        }
    }
}
