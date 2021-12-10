using System.Collections.Generic;
using UnityEngine;

public class DashObjectPool : MonoBehaviour
{
    private List<FallingBase> pool;
    private Transform poolRoot;
    private FallingBase pooledDashObject;
    public string PoolID { get; set; }

    public void SetPoolRoot(Transform newPoolRoot)
    {
        poolRoot = newPoolRoot;
        //Debug.Log("DashObjectPool.DashObjectPool()\n");
    }

    public void CreatePool(FallingBase dashObject, int quantity)
    {
       // Debug.Log($"DashObjectPool.CreatePool({dashObject.name})\n");
        pooledDashObject = dashObject;

        pool = new List<FallingBase>();
        AddToPool(quantity);
    }

    private void AddToPool(int quantity)
    {
        for (int i = 0; i < quantity; ++i)
        {
            // instantiate the object, set
            var newDash = CreateNewInstance(pooledDashObject);
            newDash.Initialize();
            newDash.HideInstantly();

            pool.Add(newDash);
        }
    }

    public FallingBase GetObjectFromPool()
    {
        foreach (var dashObjectBase in pool)
        {
            if (!dashObjectBase.IsUsed())
            {
                return dashObjectBase;
            }
        }

        // pool has run out, instantiate 5 more
        Debug.Log($"Expanding pool for {PoolID}\n");
        AddToPool(5);
        return GetObjectFromPool();
    }


    private FallingBase CreateNewInstance(FallingBase dashObject)
    {
        var newDash = Instantiate(dashObject, poolRoot);
        return newDash;
    }

    public void HideAllInstantly()
    {
        foreach (var dashObjectBase in pool)
        {
            dashObjectBase.HideInstantly();
        }
    }

    public void HideAll()
    {
        foreach (var dashObjectBase in pool)
        {
            dashObjectBase.Hide();
        }
    }

    public void LevelComplete()
    {
        foreach (var dashObjectBase in pool)
        {
            if (dashObjectBase.isFalling)
            {
                dashObjectBase.LevelComplete();
            }
        }
    }

    public void LifeLost()
    {
        foreach (var dashObjectBase in pool)
        {
            if (dashObjectBase.isFalling)
            {
                dashObjectBase.LifeLost();
            }
        }
    }
}