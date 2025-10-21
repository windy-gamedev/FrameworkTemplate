using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Wigi.Utilities
{
    public static class ObjectUtils
    {
        public static void AddChild(this Component component, GameObject child)
        {
            child.transform.SetParent(component.transform);
        }
        public static void AddChild(this Component component, Component child)
        {
            child.transform.SetParent(component.transform);
        }
        public static T AddChildInstant<T>(this Component component, T prefab) where T : Object
        {
            return Object.Instantiate(prefab, component.transform);
        }
        public static void RemoveChild(this Component component, Component child)
        {
            Object.Destroy(child.gameObject);
        }
        public static void DestroyObj(Component component)
        {
            Object.Destroy(component.gameObject);
        }
    }


    public static class AddressableUtil
    {
        public static bool IsValidAsset(this AssetReference asset)
        {
            return asset != null && asset.RuntimeKeyIsValid();
        }

        public static T LoadObject<T>(this AssetReferenceT<T> asset, Action<T> callback = null) where T : UnityEngine.Object
        {
            //check asset is't null and invalid
            if (!asset.IsValidAsset())
            {
                Debug.Log("Load addressable: Asset is't valid!");
                callback?.Invoke(null);
                return null;
            }

            if (asset.Asset != null)
            {
                var result = (T)asset.Asset;
                callback?.Invoke(result);
                return result;
            }
            else
            {
                if (!asset.OperationHandle.IsValid())
                    asset.LoadAssetAsync<T>();
                asset.OperationHandle.Completed += handle => { callback?.Invoke((T)handle.Result); };
            }
            return null;
        }

        public static IEnumerator<T> LoadObjectAsync<T>(this AssetReferenceT<T> asset, Action<T> callback = null) where T : UnityEngine.Object
        {
            //check asset is't null and invalid
            if (!asset.IsValidAsset())
            {
                Debug.Log("Load addressable: Asset is't valid!");
                callback?.Invoke(null);
                yield break;
            }

            //check Asset is loaded or wait load asset
            if (asset.Asset == null)
            {
                if (!asset.OperationHandle.IsValid())
                    asset.LoadAssetAsync<T>();
                yield return asset.OperationHandle.WaitForCompletion() as T;
            }

            //return result
            if (asset.Asset != null)
            {
                callback?.Invoke((T)asset.Asset);
                yield return asset.Asset as T;
            }
            else
            {
                callback?.Invoke(null);
            }
        }

        /// <summary>
        /// because SkeletonDataAsset is not UnityEngine.Object when runtime
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="callback"></param>
        /// <returns></returns>
        public static IEnumerator<T> LoadObjectAsync<T>(this AssetReference asset, Action<T> callback = null) where T : UnityEngine.Object
        {
            //check asset is't null and invalid
            if (asset == null || !asset.RuntimeKeyIsValid())
            {
                Debug.Log("Load addressable: Asset is't valid!");
                callback?.Invoke(null);
                yield break;
            }

            //check Asset is loaded or wait load asset
            if (asset.Asset == null)
            {
                if (!asset.OperationHandle.IsValid())
                    asset.LoadAssetAsync<T>();
                yield return asset.OperationHandle.WaitForCompletion() as T;
            }

            //return result
            if (asset.Asset != null)
            {
                callback?.Invoke(asset.Asset as T);
                yield return asset.Asset as T;
            }
            else
            {
                callback?.Invoke(null);
            }
        }
    }
}
