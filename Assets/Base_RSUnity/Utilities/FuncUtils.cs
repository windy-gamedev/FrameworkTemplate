using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wigi.Utilities
{
    public static class FuncUtils
    {
        public static Coroutine CallDelay(this MonoBehaviour monoBehaviour, float delay, Action OnCallback)
        {
            IEnumerator IERunAnimator()
            {
                yield return new WaitForSeconds(delay);
                OnCallback?.Invoke();
            }
            return monoBehaviour.StartCoroutine(IERunAnimator());
        }

        public static Coroutine CallLoop(this MonoBehaviour monoBehaviour, float loopDelay, Action OnCallback)
        {
            return CallDelayLoop(monoBehaviour, 0, loopDelay, OnCallback);
        }
        public static Coroutine CallDelayLoop(this MonoBehaviour monoBehaviour, float delay, float loopDelay, Action OnCallback)
        {
            IEnumerator IERunAnimator()
            {
                if(delay > 0)
                    yield return new WaitForSeconds(delay);

                while (true)
                {
                    yield return new WaitForSeconds(loopDelay);
                    OnCallback?.Invoke();
                }
            }
            return monoBehaviour.StartCoroutine(IERunAnimator());
        }

        public static Coroutine UpdateDuration(this MonoBehaviour monoBehaviour, float duration, float delay = 0, System.Action<float> OnUpdate = null, System.Action OnComplete = null)
        {
            IEnumerator IERunAnimator()
            {
                yield return new WaitForSeconds(delay);
                float start = Time.time;
                OnUpdate?.Invoke(0);
                while (Time.time <= start + duration)
                {
                    float time = (Time.time - start) / duration;
                    OnUpdate?.Invoke(Mathf.Clamp01(time));
                    yield return null;
                }
                OnUpdate?.Invoke(1);
                OnComplete?.Invoke();
            }
            return monoBehaviour.StartCoroutine(IERunAnimator());
        }

        public static Coroutine UpdateRepeatDelay(this MonoBehaviour monoBehaviour, float repeatTime, float delay, Action<int> OnRepeat)
        {
            IEnumerator IERunAnimator()
            {
                float start = Time.time;
                int t = 0;
                while (true)
                {
                    yield return new WaitForSeconds(delay);
                    if (Time.time >= start)
                    {
                        start += repeatTime;
                        t++;
                        OnRepeat?.Invoke(t);
                    }
                    yield return null;
                }
            }
            return monoBehaviour.StartCoroutine(IERunAnimator());
        }

        public static Coroutine UpdateRepeat(this MonoBehaviour monoBehaviour, float delay, float repeatTime, Action<int> OnRepeat)
        {
            IEnumerator IERunAnimator()
            {
                yield return new WaitForSeconds(delay);
                float start = Time.time;
                int t = 0;
                while (true)
                {
                    if (Time.time >= start)
                    {
                        start += repeatTime;
                        t++;
                        OnRepeat?.Invoke(t);
                    }
                    yield return null;
                }
            }
            return monoBehaviour.StartCoroutine(IERunAnimator());
        }

        public static Coroutine UpdateRepeat(this MonoBehaviour monoBehaviour, float delay, int count, float repeatTime, System.Action<int> OnRepeat = null, System.Action OnComplete = null)
        {
            IEnumerator IERunAnimator()
            {
                yield return new WaitForSeconds(delay);
                float start = Time.time;
                int t = 0;
                while (count > t)
                {
                    if (Time.time >= start)
                    {
                        start += repeatTime;
                        t++;
                        OnRepeat?.Invoke(t);
                    }
                    yield return null;
                }
                OnComplete?.Invoke();
            }
            return monoBehaviour.StartCoroutine(IERunAnimator());
        }
        public static Coroutine UpdateRepeat(this MonoBehaviour monoBehaviour, float delay, float repeatTimeMin, float repeatTimeMax, Action<int> OnRepeat)
        {
            IEnumerator IERunAnimator()
            {
                yield return new WaitForSeconds(delay);
                float start = Time.time;
                int t = 0;
                while (true)
                {
                    if (Time.time >= start)
                    {
                        start += UnityEngine.Random.Range(repeatTimeMin, repeatTimeMax);
                        t++;
                        OnRepeat?.Invoke(t);
                    }
                    yield return null;
                }
            }
            return monoBehaviour.StartCoroutine(IERunAnimator());
        }


        public static Coroutine UpdateInfinity(this MonoBehaviour monoBehaviour, float delay, System.Action OnUpdate)
        {
            IEnumerator IERunAnimator()
            {
                yield return new WaitForSeconds(delay);
                while (true)
                {
                    OnUpdate?.Invoke();
                    yield return null;
                }
            }
            return monoBehaviour.StartCoroutine(IERunAnimator());
        }
    }
}
