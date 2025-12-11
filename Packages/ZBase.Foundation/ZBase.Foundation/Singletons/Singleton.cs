using System;
using System.Collections.Generic;
using UnityEditor;

namespace ZBase.Foundation.Singletons
{
    /// <summary>
    /// <para>Designed for decoupling Singleton pattern and user classes.</para>
    /// <para>Usage: <see cref="Singleton"/>.Of&lt;MyClass&gt;()</para>
    /// </summary>
    public static partial class Singleton
    {
        private readonly static Dictionary<Type, object> s_instances = new();
    
        public static T Of<T>() where T : class, new()
            => GetInstance(() => new T());

        public static T Of<T>(Func<T> instantiator) where T : class
        {
            if (instantiator == null)
                throw new ArgumentNullException(nameof(instantiator));

            return GetInstance(instantiator);
        }
    
        private static T GetInstance<T>(Func<T> instantiator)
        {
            var type = typeof(T);
        
            if (s_instances.TryGetValue(type, out var obj) == false)
            {
                var result = instantiator();
                s_instances.Add(type, result);
                return result;
            }
        
            if (obj is not T instanceT)
            {
                instanceT = instantiator();
                s_instances[type] = instanceT;
            
                UnityEngine.Debug.LogError($"Registered instance does not match type '{type}' thus is replaced.");
            }
        
            return instanceT;
        }

#if UNITY_EDITOR
        [InitializeOnEnterPlayMode]
        public static void Reset()
        {
            s_instances.Clear();
        }
#endif
    }
}
