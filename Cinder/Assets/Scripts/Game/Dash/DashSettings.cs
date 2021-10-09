using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Scenes.Effects.Dash
{
    [CreateAssetMenu(fileName = "DashSettings", menuName = "ScriptableObjects/DashSettings", order = 1)]
    public class DashSettings : ScriptableObject
    {
        [SerializeField]
        public List<DashPoolConfig> poolConfigs ;

        private static DashSettings _instance;


        public static DashSettings Instance()
        {
#if UNITY_EDITOR
            if (!_instance)
            {
                var configsGUIDs = AssetDatabase.FindAssets("t:" + typeof(DashSettings).Name);
                if (configsGUIDs.Length > 0)
                {
                    var guid = configsGUIDs[0];
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    _instance = AssetDatabase.LoadAssetAtPath<DashSettings>(path);
                }
            }
#endif

            if (!_instance)
            {
                Debug.LogError("Settings not found, check that one has been created\n");
            }

            return _instance;
        }
    }
}