using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTPS
{
    public static class OOPTransformUtility 
    {
        public static Transform FindChildByName(string name, Transform parent)
        {
            if (parent.name == name)
                return parent;

            int childCOunt = parent.childCount;
            for(int i =0; i < childCOunt; ++i)
            {
                var child = parent.GetChild(i);
                var result = FindChildByName(name, child);
                if (result != null)
                    return result;
            }

            return null;
        }
    }
}
