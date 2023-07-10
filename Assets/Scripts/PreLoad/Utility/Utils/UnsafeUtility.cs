using UnityEngine;
using System.Collections;

namespace Utility
{
    public static class UnsafeUtility
    {
        struct Shell<T> where T : struct
        {
            public int intValue;
            public T enumValue;
        }

        public static int EnumToInt32<TEnum>(TEnum pEnum) where TEnum : struct
        {
            Shell<TEnum> shell;
            shell.enumValue = pEnum;
            unsafe
            {
                int* pInt = &shell.intValue;
                pInt += 1;
                return *pInt;
            }
        }
    } 

}