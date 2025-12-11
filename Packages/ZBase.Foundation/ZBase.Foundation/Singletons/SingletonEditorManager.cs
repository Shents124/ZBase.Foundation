using System.Collections.Generic;
using UnityEditor;
using UnityEngine.Scripting;

namespace ZBase.Foundation.Singletons
{
#if UNITY_EDITOR
    public class SingletonEditorManager
    {
        private readonly static List<IResetStaticsOnDomainReload > s_instances = new();

        [InitializeOnEnterPlayMode, Preserve]
        private static void InitWhenDomainReloadDisabled()
        {
            var instances = s_instances;

            foreach (var instance in instances)
            {
                instance?.ResetStaticsOnDomainReload();
            }
        
            instances.Clear();
        }

        public static void Register(IResetStaticsOnDomainReload instance)
        {
            s_instances.Add(instance);
        }
    }
#endif
}