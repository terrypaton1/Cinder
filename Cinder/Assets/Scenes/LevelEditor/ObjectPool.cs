using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    // hold pools objects of all types
    // can request an object of BrickType  or NonBrick type
    // if not enough objects exist, then create more
    // remove from active play back into the pool
    // Just make it a fairly dumb pool
    // perhaps every level flush out prefabs that haven't been requested in a while (extra params)

    [SerializeField]
    private LevelSettings levelSettings;

    [System.NonSerialized]
    public List<BrickBase> poolObjects = new List<BrickBase>();

    [System.NonSerialized]
    public List<NonBrick> nonBrickPoolObjects = new List<NonBrick>();

    private GameObject inactiveHolder;

    private void OnEnable()
    {
        poolObjects = new List<BrickBase>();
    }

    public BrickBase GetBrick(BrickType type, Transform holder)
    {
        var brickPrefab = GetBrickPrefab(type);
        // check if the prefab is in the poolObjects list
        var brickBase = TryGetFromPool(type);

        if (brickBase != null)
        {
            return brickBase;
        }

        var brick = Instantiate(brickPrefab, holder, true);

        poolObjects.Add(brick);
        return brick;
    }

    private BrickBase GetBrickPrefab(BrickType type)
    {
        foreach (var brick in levelSettings.brickPrefabs)
        {
            if (brick.brickType == type)
            {
                return brick;
            }
        }

        return null;
    }

    private BrickBase TryGetFromPool(BrickType type)
    {
        if (poolObjects == null)
        {
            poolObjects = new List<BrickBase>();
        }

        foreach (var poolObject in poolObjects)
        {
            if (poolObject == null)
            {
                continue;
            }

            if (poolObject.brickType != type)
            {
                continue;
            }

            if (!poolObject.pool_inUse)
            {
                return poolObject;
            }
        }

        return null;
    }


    public void UnloadAllLevels()
    {
        if (nonBrickPoolObjects != null || nonBrickPoolObjects.Count > 0)
        {
            foreach (var poolObject in nonBrickPoolObjects)
            {
                if (poolObject != null && poolObject.pool_inUse)
                {
                    ReturnToNonBrickPool(poolObject);
                }
            }
        }

        if (poolObjects != null || poolObjects.Count > 0)
        {
            foreach (var poolObject in poolObjects)
            {
                if (poolObject != null && poolObject.pool_inUse)
                {
                    ReturnToPool(poolObject);
                }
            }
        }
    }

    public NonBrick GetNonBrick(NonBrickType type, Transform holder)
    {
        var brickPrefab = GetNonBrickPrefab(type);
        var nonBrick = TryGetNonBrickFromPool(type);

        if (nonBrick != null)
        {
            return nonBrick;
        }
        nonBrick = Instantiate(brickPrefab, holder, true);
        nonBrickPoolObjects.Add(nonBrick);
        return nonBrick;
    }

    private NonBrick TryGetNonBrickFromPool(NonBrickType type)
    {
        if (nonBrickPoolObjects == null)
        {
            nonBrickPoolObjects = new List<NonBrick>();
        }

        foreach (var poolObject in levelSettings.nonBrickPrefabs)
        {
            if (poolObject == null)
            {
                continue;
            }

            if (poolObject.nonBrickType != type)
            {
                continue;
            }

            if (!poolObject.pool_inUse)
            {
                return poolObject;
            }
        }

        return null;
    }


    public void ReturnToNonBrickPool(NonBrick nonBrick)
    {
        TestCreateInactiveHolder();
        nonBrick.pool_inUse = false;
        nonBrick.Hide();
        nonBrick.transform.SetParent(inactiveHolder.transform);
    }

    public void ReturnToPool(BrickBase brick)
    {
        TestCreateInactiveHolder();
        brick.pool_inUse = false;
        brick.Hide();
        brick.transform.SetParent(inactiveHolder.transform);
    }


    private NonBrick GetNonBrickPrefab(NonBrickType type)
    {
        foreach (var nonBrick in levelSettings.nonBrickPrefabs)
        {
            if (nonBrick.nonBrickType == type)
            {
                return nonBrick;
            }
        }

        return null;
    }

    public void TestCreateInactiveHolder()
    {
        if (inactiveHolder != null)
        {
            return;
        }

        inactiveHolder = new GameObject("inactiveHolder");
        inactiveHolder.transform.SetParent(transform);
    }
}