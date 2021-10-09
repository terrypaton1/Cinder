using UnityEngine;

namespace Scenes.Effects.Dash
{
    public abstract class DashObjectBase : MonoBehaviour
    {
        public static string dashID;
        private bool isUsed;

        public virtual string GetDashID()
        {
            return dashID;
        }

// todo force inheritance
        public abstract void Initialize();
        public abstract void Show();
        public abstract void Hide();

        public virtual void HideInstantly()
        {
            SetUnused();
        }

        public virtual void SetPosition(Vector2 position)
        {
            transform.position = position;
        }

        public bool IsUsed()
        {
            return isUsed;
        }

        protected void SetUsed()
        {
            isUsed = true;
        }

        protected void SetUnused()
        {
            isUsed = false;
        }
    }
}