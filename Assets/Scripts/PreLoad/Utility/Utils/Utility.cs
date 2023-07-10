using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Utility
{
    public static class Utility
    {

        public static Color HexToColor(string hexString)
        {
            if (hexString.IndexOf('#') != -1)
                hexString = hexString.Replace("#", "");

            int r, g, b = 0;

            r = int.Parse(hexString.Substring(0, 2), NumberStyles.AllowHexSpecifier);
            g = int.Parse(hexString.Substring(2, 2), NumberStyles.AllowHexSpecifier);
            b = int.Parse(hexString.Substring(4, 2), NumberStyles.AllowHexSpecifier);

            return new Color(r / 255f, g / 255f, b / 255f);
        }
        public static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp = lhs;
            lhs = rhs;
            rhs = temp;
        }
        public static System.Object ByteToObject(Byte[] buffer)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream(buffer))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    stream.Position = 0;
                    return binaryFormatter.Deserialize(stream);
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }

            return null;
        }

        public static Byte[] ObjectToByte(System.Object obj)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, obj);
                    return stream.ToArray();
                }
            }
            catch (Exception exception)
            {
                Debug.LogError(exception);
            }

            return null;
        }


    }

    public enum Facing2D
    {
        Right, Left
    }
    [Serializable]
    public class CustomVector3
    {
        public Single x, y, z;
        public CustomVector3(Int32 x, Int32 y, Int32 z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        public CustomVector3(Vector3 vector)
        {
            this.x = vector.x;
            this.y = vector.y;
            this.z = vector.z;
        }
    }
    [Serializable]
    public class TransformData
    {
        public CustomVector3 position;
        public CustomVector3 rotation;
        public CustomVector3 scale;

        public TransformData()
        {
            Debug.LogError("Empty TransformData has been initialized");
            position = new CustomVector3(0, 0, 0);
            rotation = new CustomVector3(0, 0, 0);
            scale = new CustomVector3(1, 1, 1);
        }
        public TransformData(Transform transform)
        {
            this.position = new CustomVector3(transform.position);
            this.rotation = new CustomVector3(transform.localEulerAngles);
            this.scale = new CustomVector3(transform.localScale);
        }

    }
    public static class InterfaceUtility
    {
        public static T[] FindObjectsOfType<T>(GameObject pTargetObject = null) where T : class
        {
            if (pTargetObject != null)
                return pTargetObject.GetComponentsInChildren<T>();

            return GameObject.FindObjectsOfType<MonoBehaviour>().OfType<T>().ToArray();
        }

    }

    public static class UIUtility
    {
        public static Vector2 WorldPositionToCanvasPosition(RectTransform pCanvasRect, GameObject pGameObjet)
        {
            return WorldPositionToCanvasPosition(pCanvasRect, pGameObjet.transform.position);
        }

        public static Vector2 WorldPositionToCanvasPosition(RectTransform pCanvasRect, Vector3 pPosition)
        {
            Vector2 viewportPosition = Camera.main.WorldToViewportPoint(pPosition);

            Vector2 result = new Vector2(
            ((viewportPosition.x * pCanvasRect.sizeDelta.x) - (pCanvasRect.sizeDelta.x * 0.5f)),
            ((viewportPosition.y * pCanvasRect.sizeDelta.y) - (pCanvasRect.sizeDelta.y * 0.5f))
            );

            return result;
        }
    }

    public static class EnumUtility
    {
        public static T GetEnumByName<T>(System.String EnumName) where T : struct
        {
            T myEnum;
            try
            {
                myEnum = (T)System.Enum.Parse(typeof(T), EnumName);
            }
            catch (Exception ex)
            {
                Debug.LogError($"ENUM PARSE EXCEPTION at {EnumName} from , {typeof(T)} :: {ex}");
                throw;
            }
            return myEnum;
        }
        public static T GetLastEnumValue<T>() where T : struct
        {
            T lastType = System.Enum.GetValues(typeof(T)).Cast<T>().Max();
            return lastType;
        }
    }

    public static class InvokeUtility
    {
        public static Action<T, object> BuildUntypedSetter<T>(MemberInfo memberInfo)
        {
            Type targetType = memberInfo.DeclaringType;
            var exInstance = Expression.Parameter(targetType, "t");

            var exMemberAccess = Expression.MakeMemberAccess(exInstance, memberInfo);

            var exValue = Expression.Parameter(typeof(object), "p");
            var exConvertedValue = Expression.Convert(exValue, GetUnderlyingType(memberInfo));
            var exBody = Expression.Assign(exMemberAccess, exConvertedValue);

            var lambda = Expression.Lambda<Action<T, object>>(exBody, exInstance, exValue);
            var action = lambda.Compile();
            return action;
        }

        private static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    return null;
            }
        }
    }
}
