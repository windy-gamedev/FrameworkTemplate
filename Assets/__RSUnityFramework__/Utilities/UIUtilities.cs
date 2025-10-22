using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Wigi.Utilities
{
    public static class UIUtilities
    {
        #region Get Component
        public static bool GetVisible(this GameObject gameObject)
        {
            return gameObject.activeSelf;
        }
        public static bool GetVisible(this Component component)
        {
            return component.GetActive();
        }
        public static bool GetActive(this Component component)
        {
            return component.gameObject.activeSelf;
        }
        public static void SetVisible(this GameObject gameObject, bool visible)
        {
            gameObject.SetActive(visible);
        }
        public static void SetVisible(this Component component, bool visible)
        {
            component.SetActive(visible);
        }
        public static void SetActive(this Component component, bool visible)
        {
            component.gameObject.SetActive(visible);
        }

        public static GameObject GetChildByName(this GameObject gameObject, string name)
        {
            return gameObject.transform.GetChildByName(name);
        }
        public static T GetChildByName<T>(this GameObject gameObject, string name)
        {
            return gameObject.transform.GetChildByName<T>(name);
        }
        public static GameObject GetChildByName(this Component component, string name)
        {
            return component.transform.GetChildByName(name);
        }
        public static T GetChildByName<T>(this Component component, string name)
        {
            return component.transform.GetChildByName<T>(name);
        }
        public static GameObject GetChildByName(this Transform transform, string name)
        {
            Transform trans = transform.Find(name);
            if(trans != null)
                return trans.gameObject;
            return null;
        }
        public static T GetChildByName<T>(this Transform transform, string name)
        {
            var transObj = transform.Find(name);
            return transObj.GetComponent<T>();
        }

        public static GameObject GetComponentByName(string name)
        {
            GameObject obj = GameObject.Find(name);
            return obj;
        }
        public static T GetComponentByName<T>(string name)
        {
            GameObject obj = GameObject.Find(name);
            return obj.GetComponent<T>();
        }
        #endregion

        #region Position
        //World
        public static Vector2 GetWorldCenterPos(this Transform transform)
        {
            var rectTransform = transform as RectTransform;
            if (rectTransform)
            {
                var pos = rectTransform.position;
                return new Vector2(pos.x + rectTransform.GetWidth() * rectTransform.lossyScale.x * (0.5f - rectTransform.pivot.x), pos.y + rectTransform.GetHeight() * rectTransform.lossyScale.y * (0.5f - rectTransform.pivot.y));
            }
            return GetWorldPosition(transform);
        }
        public static Vector2 GetWorldCenterPos(this GameObject gameObject)
        {
            var rect = gameObject.GetComponent<RectTransform>();
            if (rect)
            {
                var pos = rect.position;
                return new Vector2(pos.x + gameObject.GetWidth() * rect.lossyScale.x * (0.5f - rect.pivot.x), pos.y + gameObject.GetHeight() * rect.lossyScale.y * (0.5f - rect.pivot.y));
            }
            return GetWorldPosition(gameObject);
        }
        public static Vector3 GetWorldPosition(this GameObject gameObject)
        {
            return gameObject.transform.position;
        }
        public static Vector3 GetWorldPosition(this Component component)
        {
            return component.transform.position;
        }
        public static float GetWorldPositionX(this Component component)
        {
            return component.transform.position.x;
        }
        public static float GetWorldPositionY(this Component component)
        {
            return component.transform.position.y;
        }
        public static void SetWorldPosition(this GameObject gameObject, Vector3 position)
        {
            gameObject.transform.position = position;
        }
        public static void SetWorldPosition(this Component component, Vector3 position)
        {
            component.transform.position = position;
        }
        public static void SetWorldPosition(this Component component, float x, float y)
        {
            var pos = component.transform.position;
            pos.x = x;
            pos.y = y;
            component.transform.position = pos;
        }
        public static void SetWorldPositionX(this Component component, float x)
        {
            var pos = component.transform.position;
            pos.x = x;
            component.transform.position = pos;
        }
        public static void SetWorldPositionY(this Component component, float y)
        {
            var pos = component.transform.position;
            pos.y = y;
            component.transform.position = pos;
        }
        public static void SetWorldPositionX(this GameObject gameObject, float x)
        {
            var pos = gameObject.transform.position;
            pos.x = x;
            gameObject.transform.position = pos;
        }
        public static void SetWorldPositionY(this GameObject gameObject, float y)
        {
            var pos = gameObject.transform.position;
            pos.y = y;
            gameObject.transform.position = pos;
        }

        //Set position by anchor of object and size of parent
        public static float GetUIPositionX(this IClippable component)
        {
            return component.rectTransform.anchoredPosition.x;
        }
        public static float GetUIPositionY(this IClippable component)
        {
            return component.rectTransform.anchoredPosition.y;
        }
        public static Vector2 GetUIPosition(this Component component)
        {
            return GetUIPosition(component.gameObject);
        }
        public static Vector2 GetUIPosition(this GameObject gameObject)
        {
            var recTrans = gameObject.GetComponent<RectTransform>();
            if(recTrans != null)
                return recTrans.anchoredPosition;
            return Vector2.zero;
        }
        public static void SetUIPositionX(this IClippable component, float x)
        {
            var pos = component.rectTransform.anchoredPosition;
            pos.x = x;
            component.rectTransform.anchoredPosition = pos;
        }
        public static void SetUIPositionY(this IClippable component, float y)
        {
            var pos = component.rectTransform.anchoredPosition;
            pos.y = y;
            component.rectTransform.anchoredPosition = pos;
        }
        public static void SetUIPosition(this Component component, Vector2 pos)
        {
            SetUIPosition(component.gameObject, pos);
        }
        public static void SetUIPosition(this GameObject gameObject, Vector2 pos)
        {
            var recTrans = gameObject.GetComponent<RectTransform>();
            if (recTrans != null)
                recTrans.anchoredPosition = pos;
        }

        //Local
        public static Vector3 GetLocalPosition(this Component component)
        {
            return component.transform.localPosition;
        }
        public static float GetLocalPositionX(this GameObject gameObject)
        {
            return gameObject.transform.localPosition.x;
        }
        public static float GetLocalPositionX(this Component component)
        {
            return component.transform.localPosition.x;
        }
        public static float GetLocalPositionY(this GameObject gameObject)
        {
            return gameObject.transform.localPosition.y;
        }
        public static float GetLocalPositionY(this Component component)
        {
            return component.transform.localPosition.y;
        }

        //SET
        public static void SetLocalPosition(this Component component, Vector3 position)
        {
            component.transform.localPosition = position;
        }
        public static void SetLocalPositionX(this Component component, float x)
        {
            var pos = component.transform.localPosition;
            pos.x = x;
            component.transform.localPosition = pos;
        }
        public static void SetLocalPositionY(this Component component, float y)
        {
            var pos = component.transform.localPosition;
            pos.y = y;
            component.transform.localPosition = pos;
        }
        public static void SetLocalPositionZ(this Component component, float z)
        {
            var pos = component.transform.localPosition;
            pos.z = z;
            component.transform.localPosition = pos;
        }

        public static void SetLocalPositionX(this GameObject gameObject, float x)
        {
            var pos = gameObject.transform.localPosition;
            pos.x = x;
            gameObject.transform.localPosition = pos;
        }
        public static void SetLocalPositionY(this GameObject gameObject, float y)
        {
            var pos = gameObject.transform.localPosition;
            pos.y = y;
            gameObject.transform.localPosition = pos;
        }
        #endregion

        #region Scale 2D
        //Scale
        public static void SetLocalScale(this Component gameObject, float scale)
        {
            var scaleTemp = gameObject.transform.localScale;
            scaleTemp.x = scale;
            scaleTemp.y = scale;
            gameObject.transform.localScale = scaleTemp;
        }
        public static void SetLocalScale(this Component gameObject, float x, float y)
        {
            var scale = gameObject.transform.localScale;
            scale.x = x;
            scale.y = y;
            gameObject.transform.localScale = scale;
        }
        public static void SetLocalScaleX(this Component gameObject, float x)
        {
            var scale = gameObject.transform.localScale;
            scale.x = x;
            gameObject.transform.localScale = scale;
        }
        public static void SetLocalScaleY(this Component gameObject, float y)
        {
            var scale = gameObject.transform.localScale;
            scale.y = y;
            gameObject.transform.localScale = scale;
        }
        #endregion

        #region Rotation
        //GET
        public static float GetRotationUI(this Component component)
        {
            return component.GetLocalRotationZ();
        }
        public static Vector3 GetLocalRotation(this Component component)
        {
            return component.transform.localEulerAngles;
        }
        public static float GetLocalRotationX(this Component component)
        {
            return component.transform.localEulerAngles.x;
        }
        public static float GetLocalRotationY(this Component component)
        {
            return component.transform.localEulerAngles.y;
        }
        public static float GetLocalRotationZ(this Component component)
        {
            return component.transform.localEulerAngles.z;
        }

        //SET
        public static void SetRotationUI(this Component component, float angle)
        {
            component.SetLocalRotationX(angle);
        }

        public static void SetLocalRotation(this Component component, float x, float y)
        {
            var angle = component.transform.localEulerAngles;
            angle.x = x;
            angle.y = y;
            component.transform.localEulerAngles = angle;
        }
        public static void SetLocalRotationX(this Component component, float x)
        {
            var angle = component.transform.localEulerAngles;
            angle.x = x;
            component.transform.localEulerAngles = angle;
        }
        public static void SetLocalRotationY(this Component component, float y)
        {
            var angle = component.transform.localEulerAngles;
            angle.y = y;
            component.transform.localEulerAngles = angle;
        }
        public static void SetLocalRotationZ(this Component component, float z)
        {
            var angle = component.transform.localEulerAngles;
            angle.z = z;
            component.transform.localEulerAngles = angle;
        }
        #endregion

        #region Width and Height
        public static void SetWidth(this Component gameObject, float width)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
        public static void SetWidth(this GameObject gameObject, float width)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        }
        public static void SetHeight(this Component gameObject, float height)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
        public static void SetHeight(this GameObject gameObject, float height)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }

        public static void SetWorldRect(this Component component, Rect worldRect)
        {
            component.gameObject.SetWorldRect(worldRect);
        }
        public static void SetWorldRect(this GameObject gameObject, Rect worldRect)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform == null)
                throw new ArgumentNullException(nameof(rectTransform));
            rectTransform.SetWorldRect(worldRect);
        }
        public static void SetWorldRect(this RectTransform rectTransform, Rect worldRect)
        {
            Vector2 localPosition = rectTransform.parent.InverseTransformPoint(worldRect.position);
            Vector2 localTop = rectTransform.parent.InverseTransformPoint(new Vector2(worldRect.xMax, worldRect.yMax));
            Vector2 localSize = localTop - localPosition;

            rectTransform.SetLocalPosition(localPosition + localSize.MultiplyScale(rectTransform.pivot));
            SetSizeWithCurrentAnchors(rectTransform, localSize);
        }
        public static void SetLocalRect(this Component component, Rect localRect)
        {
            component.gameObject.SetLocalRect(localRect);
        }
        public static void SetLocalRect(this GameObject gameObject, Rect localRect)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform == null)
                throw new ArgumentNullException(nameof(rectTransform));
            rectTransform.SetLocalRect(localRect);
        }
        public static void SetLocalRect(this RectTransform rectTransform, Rect localRect)
        {
            SetSizeWithCurrentAnchors(rectTransform, localRect.size);
            rectTransform.SetLocalPosition(localRect.position + localRect.size.MultiplyScale(rectTransform.pivot));
        }

        //Get Width
        public static float GetWidth(this Component gameObject)
        {
            return GetSizeWithCurrentAnchors(gameObject.GetComponent<RectTransform>(), RectTransform.Axis.Horizontal);
        }
        //Get Height
        public static float GetHeight(this Component gameObject)
        {
            return GetSizeWithCurrentAnchors(gameObject.GetComponent<RectTransform>(), RectTransform.Axis.Vertical);
        }
        public static float GetWidth(this GameObject gameObject)
        {
            return GetSizeWithCurrentAnchors(gameObject.GetComponent<RectTransform>(), RectTransform.Axis.Horizontal);
        }
        public static float GetHeight(this GameObject gameObject)
        {
            return GetSizeWithCurrentAnchors(gameObject.GetComponent<RectTransform>(), RectTransform.Axis.Vertical);
        }
        public static float GetWidth(this RectTransform gameObject)
        {
            return GetSizeWithCurrentAnchors(gameObject, RectTransform.Axis.Horizontal);
        }
        public static float GetHeight(this RectTransform gameObject)
        {
            return GetSizeWithCurrentAnchors(gameObject, RectTransform.Axis.Vertical);
        }
        static float GetSizeWithCurrentAnchors(RectTransform rectTransform, RectTransform.Axis axis)
        {
            if (rectTransform == null)
                throw new ArgumentNullException(nameof(rectTransform));

            int axisI = (int)axis;
            return rectTransform.sizeDelta[axisI] + GetParentSize(rectTransform)[axisI] * (rectTransform.anchorMax[axisI] - rectTransform.anchorMin[axisI]);

        }
        public static Rect GetWorldRect(this Component component)
        {
            return GetWorldRect(component.gameObject);
        }
        public static Rect GetWorldRect(this GameObject gameObject)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform == null)
                throw new ArgumentNullException(nameof(rectTransform));

            //local rect is rect of self object
            var localRect = rectTransform.rect;
            return localRect.LocalToWorldRect(rectTransform);
        }

        public static Size GetWorldSize(this Component component)
        {
            return GetWorldSize(component.gameObject);
        }
        public static Size GetWorldSize(this GameObject gameObject)
        {
            var rectTransform = gameObject.GetComponent<RectTransform>();
            if (rectTransform == null)
                throw new ArgumentNullException(nameof(rectTransform));

            var localRect = rectTransform.rect;
            Vector2 worldPos = rectTransform.TransformPoint(localRect.position);
            Vector2 worldTop = rectTransform.TransformPoint(new Vector2(localRect.xMax, localRect.yMax));
            return (worldTop - worldPos).Size();
        }
        public static Size GetSize(this GameObject gameObject)
        {
            return GetSizeWithCurrentAnchors(gameObject.GetComponent<RectTransform>());
        }
        public static Size GetSize(this Component component)
        {
            return GetSizeWithCurrentAnchors(component.GetComponent<RectTransform>());
        }
        static Size GetSizeWithCurrentAnchors(RectTransform rectTransform)
        {
            if(rectTransform == null)
                throw new ArgumentNullException(nameof(rectTransform));
            return new Size(rectTransform.sizeDelta + GetParentSize(rectTransform) * (rectTransform.anchorMax - rectTransform.anchorMin));

        }
        static void SetSizeWithCurrentAnchors(RectTransform rectTransform, Vector2 size)
        {
            Vector2 vector = size - GetParentSize(rectTransform).MultiplyScale(rectTransform.anchorMax - rectTransform.anchorMin);
            rectTransform.sizeDelta = vector;
        }

        static Vector2 GetParentSize(RectTransform rectTransform)
        {
            RectTransform rect = rectTransform.parent as RectTransform;
            if (rect == null)
            {
                return Vector2.zero;
            }
            return rect.rect.size;
        }
        #endregion

        #region Color and Alpha
        /// <summary>
        /// Set alpha color for graphic
        /// </summary>
        /// <param name="target"></param>
        /// <param name="alpha">0 - 1f</param>
        public static void SetAlpha(this Graphic target, float alpha)
        {
            var rawColor = target.color;
            rawColor.a = alpha;
            target.color = rawColor;
        }
        public static void SetAlpha(this SpriteRenderer target, float alpha)
        {
            var rawColor = target.color;
            rawColor.a = alpha;
            target.color = rawColor;
        }
        public static void SetAlpha(this Material material, float alpha)
        {
            var rawColor = material.color;
            rawColor.a = alpha;
            material.color = rawColor;
        }
        public static void SetAlpha(this CanvasGroup canvas, float alpha)
        {
            canvas.alpha = alpha;
        }
        #endregion

        #region Image and Sprite
        /// <summary>
        /// Set the scale of the sprite displayed in the size area of the image
        /// </summary>
        public static void SetScaleAutoFixImage(this Image image, float scaleFix = 1)
        {
            var sizeI = image.rectTransform.rect;
            var sizeT = image.sprite.rect;

            var rateXY = sizeT.width / sizeT.height;
            var rateImageXY = sizeI.width / sizeI.height;
            if (rateImageXY > rateXY)
            {
                image.SetLocalScale(scaleFix * rateXY / rateImageXY, scaleFix);
            }
            else
            {
                image.SetLocalScale(scaleFix, scaleFix * rateImageXY / rateXY);
            }
        }

        public static void SetScaleWidthFixImage(this Image image, float scaleFix = 1)
        {
            var sizeI = image.rectTransform.rect;
            var sizeT = image.sprite.rect;

            float rateXY = sizeT.width / sizeT.height;
            float rateImageXY = sizeI.width / sizeI.height;

            float flipX = image.rectTransform.localScale.x < 0 ? -1 : 1 ;
            float flipY = image.rectTransform.localScale.y < 0 ? -1 : 1;
            image.SetLocalScale(flipX * scaleFix, flipY * scaleFix * rateImageXY / rateXY);
        }

        public static void SetScaleHeightFixImage(this Image image, float scaleFix = 1)
        {
            var sizeI = image.rectTransform.rect;
            var sizeT = image.sprite.rect;

            float rateXY = sizeT.width / sizeT.height;
            float rateImageXY = sizeI.width / sizeI.height;

            float flipX = image.rectTransform.localScale.x < 0 ? -1 : 1;
            float flipY = image.rectTransform.localScale.y < 0 ? -1 : 1;
            image.SetLocalScale(flipX * scaleFix * rateXY / rateImageXY, flipY * scaleFix);
        }

        /// <summary>
        /// Calculator scale object 3D for keep size ui
        /// </summary>
        /// <param name="renderer"></param>
        /// <param name="scaleFix"></param>
        public static void SetKeepSizeImage3D(this SpriteRenderer renderer, Sprite sprite)
        {
            var scaleObj = renderer.transform.localScale;
            var rectOld = renderer.sprite.rect;

            var sizeObj = new Vector2(scaleObj.x * rectOld.width, scaleObj.y * rectOld.height);
            scaleObj.x = sizeObj.x / sprite.rect.width;
            scaleObj.y = sizeObj.y / sprite.rect.height;

            renderer.sprite = sprite;
            renderer.transform.localScale = scaleObj;
        }
        #endregion

        #region InputField

        public static void SetPlaceHolderText(this TMP_InputField input, string text)
        {
            input.placeholder.GetComponent<TMP_Text>().text = text;
        }
        #endregion
    }

}
