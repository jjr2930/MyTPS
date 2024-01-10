using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyTPS
{
    public class VFXPoolObject : MyTPSPoolObject
    {
        [SerializeField] float delay;
        public override void OnPoped()
        {
            base.OnPoped();

            StartCoroutine(CheckReturn());
        }

        public override void OnReturned()
        {
            base.OnReturned();

            transform.SetParent(null);

            StopAllCoroutines();
        }

        IEnumerator CheckReturn()
        {
            yield return new WaitForSeconds(delay);

            MyTPSObjectPool.Instance.ReturnOne(this.key, this);
        }
    }
}
