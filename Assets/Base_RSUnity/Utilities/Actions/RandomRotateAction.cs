using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace Wigi.Actions
{
    public class RandomRotateAction : MonoBehaviour
    {
        public float Time = 0.3f;
        public bool isRotateCurrent = true;
        [ShowIf("@!isRotateCurrent")]
        public Vector3 FromRotate;
        public Vector3 MinRotate;
        public Vector3 MaxRotate;
        public Ease EaseType;
        public bool playWhenEnable = false;

        private void OnEnable()
        {
            if (playWhenEnable)
                Play();
        }

        public void Play()
        {
            Vector3 randRotate = new Vector3();
            randRotate.x = Random.Range(MinRotate.x, MaxRotate.x);
            randRotate.y = Random.Range(MinRotate.y, MaxRotate.y);
            randRotate.z = Random.Range(MinRotate.z, MaxRotate.z);

            if (isRotateCurrent)
                FromRotate = transform.localEulerAngles;
            Vector3 endRand = FromRotate + randRotate;
            transform.DOLocalRotate(endRand, Time).SetEase(EaseType);
        }
    }
}
