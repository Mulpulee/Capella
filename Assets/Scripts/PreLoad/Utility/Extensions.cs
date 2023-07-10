using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Utility;


namespace Extensions
{
    public static class VectorExtensions
    {
        public static Vector3 GeneralizeRotation2D(this Vector3 vector)
        {
            if (vector.z > 360)
            {
                while (vector.z > 360)
                    vector.z -= 360;
                return vector;
            }
            else if (vector.z < 0)
            {
                while (vector.z < 0)
                    vector.z += 360;
                return vector;
            }

            return vector;

        }
    }

    public static class TransformExtensions
    {
        public static RectTransform AsRect(this Transform transform)
        {
            return transform as RectTransform;
        }

        public static void ClearChilds(this Transform transform)
        {
            foreach (Transform item in transform)
            {
                GameObject.Destroy(item.gameObject);
            }
        }

        public static void ClearChildsExcept(this Transform transform,Transform pExcept)
        {
            foreach (Transform item in transform)
            {
                if (pExcept == item)
                    continue;
                GameObject.Destroy(item.gameObject);
            }
        }

        public static void ApplyTransformData(this Transform transform, TransformData transformData)
        {
            transform.position = new Vector3(transformData.position.x, transformData.position.y, transformData.position.z);
            transform.localEulerAngles = new Vector3(transformData.rotation.x, transformData.rotation.y, transformData.rotation.z);
            transform.localScale = new Vector3(transformData.scale.x, transformData.scale.y, transformData.scale.z);
        }

        public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }
        public static Facing2D GetFacing2D(this Transform transform)
        {
            if (transform.lossyScale.x > 0)
                return Facing2D.Right;
            else
                return Facing2D.Left;
        }

        public static void SetFacing2D(this Transform transform, Facing2D value)
        {
            Int32 FacingScale = (value == Facing2D.Right ? 1 : -1);
            Single targetScale = Mathf.Abs(transform.localScale.x) * FacingScale;
            transform.SetScaleX(targetScale);
        }

        public static void LookAt2D(this Transform transform, Vector2 target)
        {
            target.x -= transform.position.x;
            target.y -= transform.position.y;
            Single angle = Mathf.Atan2(target.y, target.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }

        public static void LookAt2DFor(this Transform transform,Vector2 pTarget,Single pTime,Action pCallback = null)
        {
            pTarget.x -= transform.position.x;
            pTarget.y -= transform.position.y;
            Single angle = Mathf.Atan2(pTarget.y, pTarget.x) * Mathf.Rad2Deg;
            var rotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.DORotate(rotation.eulerAngles, pTime).OnComplete(()=>pCallback?.Invoke());
        }

        public static void SetScaleX(this Transform transform, Single x)
        {
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
        }
        public static void Flip(this Transform transform)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        #region GetterAndSetter
        public static Single GetX(this GameObject gameObject) => gameObject.transform.position.x;
        public static Single GetX(this MonoBehaviour gameObject) => gameObject.transform.position.x;
        public static Single GetX(this Transform transform) => transform.position.x;
        public static Single GetY(this GameObject gameObject) => gameObject.transform.position.y;
        public static Single GetY(this MonoBehaviour gameObject) => gameObject.transform.position.y;
        public static Single GetY(this Transform transform) => transform.position.y;
        public static Single GetZ(this GameObject gameObject) => gameObject.transform.position.z;
        public static Single GetZ(this MonoBehaviour gameObject) => gameObject.transform.position.z;
        public static Single GetZ(this Transform transform) => transform.position.z;
        public static void SetX(this GameObject gameObject, Single x) =>
            gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z);
        public static void SetX(this MonoBehaviour gameObject, Single x) =>
            gameObject.transform.position = new Vector3(x, gameObject.transform.position.y, gameObject.transform.position.z);
        public static void SetX(this Transform transform, Single x) =>
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        public static void SetY(this GameObject gameObject, Single y) =>
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        public static void SetY(this MonoBehaviour gameObject, Single y) =>
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, y, gameObject.transform.position.z);
        public static void SetY(this Transform transform, Single y) =>
            transform.position = new Vector3(transform.position.x, y, transform.position.z);
        public static void SetZ(this GameObject gameObject, Single z) =>
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z);
        public static void SetZ(this MonoBehaviour gameObject, Single z) =>
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, z);
        public static void SetZ(this Transform transform, Single z) =>
            transform.position = new Vector3(transform.position.x, transform.position.y, z);
        #endregion
        public static void SetPosition(this GameObject gameObject, Vector3 position)
        {
            gameObject.transform.position = position;
        }
        public static void SetPosition(this GameObject gameObject, Single x, Single y, Single z)
        {
            gameObject.transform.position = new Vector3(x, y, z);
        }

        public static void SetPosition(this Transform transform, Vector3 position)
        {
            transform.position = position;
        }
        public static void SetPosition(this Transform transform, Single x, Single y, Single z)
        {
            transform.position = new Vector3(x, y, z);
        }
    }

    public static class CollectionExtensions
    {
        public static void ChangeKey<TKey, TValue>(this IDictionary<TKey, TValue> dic, TKey fromKey, TKey toKey)
        {
            TValue value = dic[fromKey];
            dic.Remove(fromKey);
            dic[toKey] = value;
        }

        public static T Random<T>(this IList<T> list)
        {
            return list[UnityEngine.Random.Range(0, list.Count)];
        }

        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            Int32 count = enumerable.Count();
            return enumerable.ElementAt(UnityEngine.Random.Range(0, count));
        }
        
        public static IEnumerable<T> FastReverse<T>(this IList<T> items)
        {
            for (int i = items.Count - 1; i >= 0; i--)
            {
                yield return items[i];
            }
        }
        public static IEnumerable<T> FastReverse<T>(this IList<T> items,Int32 pFrom)
        {
            for (int i = pFrom; i >= 0; i--)
            {
                yield return items[i];
            }
        }
    }

    public static class SpriteExtensions
    {
        public static void SetAlpha(this SpriteRenderer spriteRenderer, Single Alpha)
        {
            Color temp = spriteRenderer.color;
            temp.a = Alpha;
            spriteRenderer.color = temp;
        }

        public static void Show(this SpriteRenderer spriteRenderer)
        {
            spriteRenderer.color = Color.white;
        }
        public static void Show(this SpriteRenderer spriteRenderer, Color pColor)
        {
            spriteRenderer.color = pColor;
        }
        public static void Hide(this SpriteRenderer spriteRenderer)
        {
            spriteRenderer.color = Color.clear;
        }
    }

    public static class StringUtility
    {
        public static Single ToFloat(this String str)
        {
            if (float.TryParse(str, out Single value))
                return value;
            Debug.LogError($"Cannot Parse float from {str}");

            return 0;
        }

        public static Byte ToByte(this String str)
        {
            if (Byte.TryParse(str, out Byte value))
                return value;
            Debug.LogError($"Cannot Parse Byte from {str}");

            return 0;
        }

        public static Int16 ToInt16(this String str)
        {
            if (Int16.TryParse(str, out Int16 value))
                return value;
            Debug.LogError($"Cannot Parse int16 from {str}");

            return 0;
        }

        public static Int32 ToInt32(this String str)
        {
            if (int.TryParse(str, out Int32 value))
                return value;
            Debug.LogError($"Cannot Parse int32 from {str}");

            return 0;
        }

        public static SByte ToSByte(this String str)
        {
            if (SByte.TryParse(str, out SByte value))
                return value;
            Debug.LogError($"Cannot Parse SByte from {str}");

            return 0;
        }

        public static Boolean ToBoolean(this String str)
        {
            if (Boolean.TryParse(str, out Boolean value))
                return value;
            Debug.LogError($"Cannot Parse Boolean from {str}");

            return false;
        }


        public static string ToSHA256Hash(this string data)
        {

            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(data));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }


        public static System.Object ParseToType(System.String value, Type type)
        {
            if (type == typeof(Int32))
                return value.ToInt32();
            else if (type == typeof(Int16))
                return value.ToInt16();
            else if (type == typeof(Single))
                return value.ToFloat();
            else if (type == typeof(Byte))
                return value.ToByte();
            else if (type == typeof(SByte))
                return value.ToSByte();
            else if (type == typeof(Boolean))
                return value.ToBoolean();
            else if (type == typeof(String))
                return value;
            else if (type.IsEnum)
                return System.Enum.Parse(type, value);
            else
            {
                Debug.LogError($"Please Implement Type : {type} in ParseToType");
                return null;
            }
        }

        public static T ToEnum<T>(this System.String str) where T : struct => EnumUtility.GetEnumByName<T>(str);
    }
}

