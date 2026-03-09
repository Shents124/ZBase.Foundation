using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZBase.Foundation
{
    public static class TypeCache
    {
        private static event Action OnClearCache;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ClearAll()
        {
            OnClearCache?.Invoke();
        }
        
        internal static void RegisterClear(Action initAction)
        {
            OnClearCache += initAction;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type Get<T>()
            => TypeCache<T>.Type;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Is<T>(this Type type)
            => type == TypeCache<T>.Type;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeHash GetHash<T>()
            => TypeCache<T>.Hash;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string GetName<T>()
            => TypeCache<T>.Name;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type Type<T>(this object _)
            => TypeCache<T>.Type;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string TypeName<T>(this object _)
            => TypeCache<T>.Name;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeHash TypeHash<T>(this object _)
            => TypeCache<T>.Hash;
    }
    
    public static class TypeCache<T>
    {
        private static Type s_type;
        private static string s_name;
        private static TypeHash s_hash;
        private static bool s_isReferenceOrContainsReference;

        static TypeCache()
        {
            Init();
            
            TypeCache.RegisterClear(Init);
        }

        
        static void Init()
        {
            s_type = typeof(T);
            s_name = s_type.Name;
            
            s_hash = (TypeHash)s_type; 
            s_isReferenceOrContainsReference = RuntimeHelpers.IsReferenceOrContainsReferences<T>();
        }

        public static Type Type => s_type;
        public static string Name => s_name;
        public static TypeHash Hash => s_hash;
        public static bool IsReferenceOrContainsReference => s_isReferenceOrContainsReference;
    }
}