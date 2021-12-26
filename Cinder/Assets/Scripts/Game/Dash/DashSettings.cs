using System.Collections.Generic;
using UnityEngine;

namespace Scenes.Effects.Dash
{
    [CreateAssetMenu(fileName = "DashSettings", menuName = "ScriptableObjects/DashSettings", order = 1)]
    public class DashSettings : ScriptableObject
    {
        [SerializeField]
        public List<DashPoolConfig> poolConfigs;
    }
}