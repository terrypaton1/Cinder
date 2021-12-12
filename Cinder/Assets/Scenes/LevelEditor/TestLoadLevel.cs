using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TestLoadLevel : MonoBehaviour
{
    [SerializeField]
    private LevelSettings levelSettings;

    [System.NonSerialized]
    public List<BrickBase> poolObjects = new List<BrickBase>();

    [System.NonSerialized]
    public List<NonBrick> nonBrickPoolObjects = new List<NonBrick>();

    [Range(1, 66)]
    public int loadLevel = 1;

    private GameObject holder;
    private GameObject inactiveHolder;

    private Dictionary<int, LevelData> cachedLevelData = new Dictionary<int, LevelData>();

    public List<BrickBase> currentLevelsBricks;
    public List<NonBrick> currentNonLevelsBricks;


    private Level levelRef;

    private void OnEnable()
    {
        CoreConnector.TestLoadLevel = this;
        poolObjects = new List<BrickBase>();
        TestCreateInactiveHolder();
    }

    public void Initialise()
    {
        CacheLevelData();
    }

    private void OnDisable()
    {
        CoreConnector.TestLoadLevel = null;
    }

    private void TestCreateInactiveHolder()
    {
        if (inactiveHolder != null)
        {
            return;
        }

        inactiveHolder = new GameObject("inactiveHolder");
        inactiveHolder.transform.SetParent(transform);
    }

    public void CacheLevelData()
    {
        if (cachedLevelData.Count > 0)
        {
            return;
        }

        cachedLevelData = new Dictionary<int, LevelData>();
        foreach (var levelDataStorage in levelSettings.levelDataStorage)
        {
            var levelJson = levelDataStorage.jsonData;
            if (levelJson != "")
            {
                var levelData = JsonUtility.FromJson<LevelData>(levelJson);
                if (levelData == null)
                {
                    Debug.Log($"LevelJson:{levelJson}");
                }

                cachedLevelData.Add(levelData.levelNumber, levelData);
            }
        }
    }

    private LevelData GetCachedLevelData(int levelNumber)
    {
        cachedLevelData.TryGetValue(levelNumber, out var output);
        return output;
    }

    public List<BrickBase> DisplayLevel(int levelNumber)
    {
        loadLevel = levelNumber;
        TestCreateInactiveHolder();
        TestCreateLevelHolder();

        CacheLevelData();
        var levelData = GetCachedLevelData(loadLevel);
        // instead of storing it as json, parse it and store it.
        currentLevelsBricks = new List<BrickBase>();
        currentNonLevelsBricks = new List<NonBrick>();
        foreach (var brickData in levelData.bricks)
        {
//            Debug.Log($"Place brick:{brickData.brickType} {brickData.position}");

            var brick = GetBrick(brickData.brickType);
            brick.pool_inUse = true;
            brick.Show();
            brick.transform.SetParent(holder.transform);
            var position = ProcessPosition(brickData.position);
            brick.transform.position = position;

            // process rotation
            var rotation = brickData.eulerRotation;
            rotation.z = Mathf.RoundToInt(rotation.z / 5.0f) * 5.0f;

            brick.transform.localEulerAngles = rotation;
            brick.transform.localScale = brickData.scale;

            currentLevelsBricks.Add(brick);
        }

        // create non bricks
        foreach (var brickData in levelData.nonBricks)
        {
            Debug.Log($"brickData.nonBrickType:{brickData.nonBrickType}");
            
            var nonBrick = GetNonBrick(brickData.nonBrickType);
            nonBrick.pool_inUse = true;
            nonBrick.Show();
            nonBrick.transform.SetParent(holder.transform);

            nonBrick.transform.position = brickData.position;

            var rotation = ProcessRotation(brickData.eulerRotation);
            nonBrick.transform.localEulerAngles = rotation;
            nonBrick.transform.localScale = brickData.scale;

            currentNonLevelsBricks.Add(nonBrick);
        }

        if (levelRef != null)
        {
            levelRef.bricks = currentLevelsBricks.ToArray();
            levelRef.nonBricks = currentNonLevelsBricks.ToArray();
        }

        return currentLevelsBricks;
    }

    private Vector3 ProcessPosition(Vector3 position)
    {
        position.x = Mathf.RoundToInt(position.x / 0.01f) * 0.01f;
        position.y = Mathf.RoundToInt(position.y / 0.01f) * 0.01f;
        return position;
    }

    private Vector3 ProcessRotation(Vector3 rotation)
    {
        rotation.z = Mathf.RoundToInt(rotation.z / 5.0f) * 5.0f;
        return rotation;
    }

    private void TestCreateLevelHolder()
    {
        var levelGameObjectName = $"Level_{loadLevel}";

        if (holder != null && holder.name == levelGameObjectName)
        {
            return;
        }

        holder = new GameObject(levelGameObjectName);
        holder.transform.SetParent(transform);
        levelRef = holder.AddComponent<Level>();
    }

    private BrickBase GetBrick(BrickType type)
    {
        var brickPrefab = GetBrickPrefab(type);
        // check if the prefab is in the poolObjects list
        var brickBase = TryGetFromPool(type);

        if (brickBase != null)
        {
            return brickBase;
        }

        var brick = Instantiate(brickPrefab, holder.transform, true);

        poolObjects.Add(brick);
        return brick;
    }


    private NonBrick GetNonBrick(NonBrickType type)
    {
        var brickPrefab = GetNonBrickPrefab(type);
        // check if the prefab is in the poolObjects list
        var brickBase = TryGetNonBrickFromPool(type);

        if (brickBase != null)
        {
            return brickBase;
        }

        var nonBrick = Instantiate(brickPrefab, holder.transform, true);
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

    public void ReturnToNonBrickPool(NonBrick nonBrick)
    {
        nonBrick.pool_inUse = false;
        nonBrick.Hide();
        nonBrick.transform.SetParent(inactiveHolder.transform);
    }

    public void ReturnToPool(BrickBase brick)
    {
        brick.pool_inUse = false;
        brick.Hide();
        brick.transform.SetParent(inactiveHolder.transform);
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

    public void UnLoadAllLevels()
    {
        // todo do the same to the nonBrickPoolObjects
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

        DestroyHolder();
    }

    private void DestroyHolder()
    {
        if (holder != null)
        {
            if (Application.isPlaying)
            {
                Destroy(holder);
            }
            else
            {
                DestroyImmediate(holder);
            }
        }
    }
}