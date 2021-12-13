using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    private ObjectPool objectPool;

    [SerializeField]
    private LevelSettings levelSettings;

    [Range(1, 66)]
    public int loadLevel = 1;

    private GameObject holder;

    private Dictionary<int, LevelData> cachedLevelData = new Dictionary<int, LevelData>();

    public List<BrickBase> currentLevelsBricks;
    public List<NonBrick> currentNonLevelsBricks;

    private Level levelRef;

    private void OnEnable()
    {
        CoreConnector.LevelManager = this;
        objectPool.TestCreateInactiveHolder();
        AddListeners();
    }

    private void AddListeners()
    {
        RemoveListeners();
        // todo add listeners for loading, unloading levels
    }

    private void RemoveListeners()
    {
    }

    public void Initialise()
    {
        CacheLevelData();
    }

    private void OnDisable()
    {
        CoreConnector.LevelManager = null;
        RemoveListeners();
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

    public void DisplayLevel(int levelNumber)
    {
        UnLoadAllLevels();
        loadLevel = levelNumber;
        TestCreateLevelHolder();

        CacheLevelData();
        var levelData = GetCachedLevelData(loadLevel);
        // instead of storing it as json, parse it and store it.
        currentLevelsBricks = new List<BrickBase>();
        currentNonLevelsBricks = new List<NonBrick>();
        foreach (var brickData in levelData.bricks)
        {
//            Debug.Log($"Place brick:{brickData.brickType} {brickData.position}");

            var brick = objectPool.GetBrick(brickData.brickType, holder.transform);
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

            var nonBrick = objectPool.GetNonBrick(brickData.nonBrickType, holder.transform);
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


    public void UnLoadAllLevels()
    {
        objectPool.UnloadAllLevels();

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