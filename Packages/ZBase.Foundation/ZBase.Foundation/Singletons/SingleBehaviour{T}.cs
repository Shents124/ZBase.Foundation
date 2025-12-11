using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZBase.Foundation.Singletons
{
    public abstract class SingleBehaviour<T> : MonoBehaviour, IResetStaticsOnDomainReload
        where T : SingleBehaviour<T>
    {
        [SerializeField]
        private SingleBehaviour.Lifetime _lifetime;

        private static T s_instance;
        
        public void ResetStaticsOnDomainReload()
        {
            s_instance = null;
        }
        
        protected void Awake()
        {
            if (this is not T instance)
            {
                ErrorInvalidType(this);
                Destroy(gameObject);
                return;
            }

            if (s_instance && s_instance != instance)
            {
                ErrorDuplicateInstance(name);
                Destroy(gameObject);
                return;
            }

            if (s_instance == false)
            {
                SingleBehaviour.Single<T>.SetInstance(s_instance = instance, _lifetime);
            }

            OnAwake();

#if UNITY_EDITOR
            SingletonEditorManager.Register(s_instance);
#endif
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected virtual void OnAwake() { }

        [Conditional("UNITY_EDITOR")]
        private static void ErrorInvalidType(MonoBehaviour context)
        {
            UnityEngine.Debug.LogError($"\"T\" is expected to be {context.GetType()}, but it is {typeof(T)}.");
        }

        [Conditional("UNITY_EDITOR")]
        private static void ErrorDuplicateInstance(string name)
        {
            UnityEngine.Debug.LogError($"An instance of {typeof(T)} has already exist. Duplicate instance on the gameobject \"{name}\" will be destroyed.");
        }
    }
}