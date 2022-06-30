using UnityEngine;

namespace Scenes.Effects.Dash
{
    public class Dash : MonoBehaviour
    {
        [SerializeField]
        private DashPool dashPool;

        public void Initialize()
        {
            dashPool.Initialize();
            dashPool.CreatePool();
        }

        public FallingBase GetDashObject(string dashID)
        {
            // get the first available pooled object
            // Todo look at using a generic factory pattern here
            var dashObjectPool = dashPool.GetObjectPool(dashID);
            var dashObject = dashObjectPool.GetObjectFromPool();
            return dashObject;
        }

        /// <summary>
        /// Used for shutting things down instantly, eg: clearing the screen
        /// </summary>
        public void HideAllInstantly()
        {
            dashPool.HideAllInstantly();
        }

        // todo add shut all down of certain types
        public void HideAll(string dashID)
        {
            var dashObjectPool = dashPool.GetObjectPool(dashID);
            dashObjectPool.HideAll();
        }

        public void LevelComplete()
        {
            dashPool.LevelComplete();
            // apply level complete to all active fall objects
        }

        public void LifeLost()
        {
            dashPool.LevelComplete();
        }
    }
}