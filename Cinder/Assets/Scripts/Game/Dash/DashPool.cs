using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace Scenes.Effects.Dash
{
    public class DashPool : MonoBehaviour
    {
        [SerializeField]
        private DashSettings settings;

        // This class acts as a collection of pools stored in a Dictionary <string ,object>
        private Dictionary<string, DashObjectPool> dashPools;

        public void Initialize()
        {
            //Debug.Log("DashPool.Initialize()\n");
        }

        public void CreatePool()
        {
            dashPools = new Dictionary<string, DashObjectPool>();
            // iterate through the configuration object, creating pools as needed.

            foreach (var config in settings.poolConfigs)
            {
                var newPool = CreateNewPool(config);
                dashPools.Add(newPool.PoolID, newPool);
            }
        }

        private DashObjectPool CreateNewPool(DashPoolConfig config)
        {
            var newPoolRoot = new GameObject($"pool_{config.dashObject.GetDashID()}");
            newPoolRoot.transform.SetParent(transform);

            var newPool = newPoolRoot.AddComponent<DashObjectPool>();
            newPool.PoolID = config.dashObject.GetDashID();
            newPool.SetPoolRoot(newPoolRoot.transform);
            newPool.CreatePool(config.dashObject, config.quantity);
            return newPool;
        }

        public DashObjectPool GetObjectPool(string dashID)
        {
            DashObjectPool dashObjectPool;
            if (dashPools.TryGetValue(dashID, out dashObjectPool))
            {
                //Debug.Log("Found\n");
            }
            else
            {
                Assert.IsNotNull(dashObjectPool, $"Couldn't find {dashID}");
            }

            return dashObjectPool;
        }

        public void HideAllInstantly()
        {
            foreach (var dashPoolKeyPair in dashPools)
            {
                var pool = dashPoolKeyPair.Value;
                pool.HideAllInstantly();
            }
        }


        public void LevelComplete()
        {
            foreach (var dashPoolKeyPair in dashPools)
            {
                var pool = dashPoolKeyPair.Value;
                pool.LevelComplete();
            }
        }
    }
}