﻿using System;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    [SerializeField]
    GameObject fallingPointsPrefabReference;

    [SerializeField]
    GameObject brickMultiHitSpritePrefabReference;

    [NonSerialized]
    public int activeBrickCount;

    List<BrickBase> BrickList = new List<BrickBase>();

    Vector3 brickTestPosition = Vector3.zero;

    float distance;

    void Awake()
    {
        //  number of children
        BrickList = new List<BrickBase>();
    }

    void OnEnable()
    {
        Messenger.AddListener(MenuEvents.RestartGame, RestartLevel);
        Messenger.AddListener(MenuEvents.NextLevel, NextLevel);
        activeBrickCount = 0;
    }

    void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.RestartGame, RestartLevel);
        Messenger.RemoveListener(MenuEvents.NextLevel, NextLevel);
    }

    void NextLevel()
    {
        BrickList = new List<BrickBase>();
    }

    void RestartLevel()
    {
//		Debug.Log("RestartLevel");
        activeBrickCount = BrickList.Count;
        // tell all bricks to reactive
        for (var i = 0; i < BrickList.Count; i++)
        {
            BrickList[i].ResetBrick();
        }
    }

    public void RegisterBrick(BrickBase _brick, bool passFallingPointsToBrick)
    {
//		Debug.Log("RegisterBrick", _brick.gameObject);
        BrickList.Add(_brick);
        activeBrickCount++;
//		Debug.Log("passFallingPointsToBrick:"+passFallingPointsToBrick);
        if (passFallingPointsToBrick)
        {
            // when a brick is registered, it is given a points object
            var _fallingPointsObject = Instantiate(fallingPointsPrefabReference, Vector3.zero, Quaternion.identity);
            _fallingPointsObject.transform.parent = transform;
            var _fallingPoints = _fallingPointsObject.GetComponent<FallingPoints>();
            var _brickScript = _brick;
            _brickScript.SetupFallingPointObject(_fallingPoints);
        }

//		Debug.Log("activeBrickCount:" + activeBrickCount);
    }

    public void BrickDestroyed(BrickBase _brick)
    {
//		Debug.Log("Destroy brick", _brick.gameObject);
        Messenger.Broadcast(GlobalEvents.BrickWasDestroyed);
        activeBrickCount--;
        GameVariables.instance.IncreaseBricksBroken();

//		Debug.Log("activeBrickCount:" + activeBrickCount);
        if (activeBrickCount < 1)
        {
            Messenger.Broadcast(MenuEvents.LevelComplete);
        }
    }

    public Vector3 GetLastBrickLocation()
    {
        if (BrickList.Count == 0)
        {
            Debug.LogError("there are no bricks left");
            Debug.Break();
        }

        return BrickList[0].transform.position;
    }

    public void TestDestroyBricksAroundTNT(Vector3 position, float range, BrickBase TheTNTBrick)
    {
        // find all bricks within radius of this one.
#if UNITY_EDITOR
//		Debug.DrawRay(position, Vector2.up * range, Color.blue, 10);
#endif
        // scan through all active bricks

        for (var i = 0; i < BrickList.Count; i++)
        {
            // make sure the tnt brick doesn't try top explode itself
            if (TheTNTBrick != BrickList[i])
            {
                if (!BrickList[i].BrickHasBeenDestroyed)
                {
                    // check if its in range
                    brickTestPosition = BrickList[i].transform.position;
                    distance = Vector3.Distance(position, brickTestPosition);
                    if (distance <= range)
                    {
                        // affect the brick
                        BrickList[i].AffectedByTNTExplosion();
                    }
                }
            }
        }
    }

    void OnDestroy()
    {
        s_Instance = null;
    }

    private static BrickManager s_Instance;

    public static BrickManager instance
    {
        get
        {
            if (s_Instance == null)
            {
                // This is where the magic happens.
                //  FindObjectOfType(...) returns the first AManager object in the scene.
                s_Instance = FindObjectOfType(typeof(BrickManager)) as BrickManager;
            }

            // If it is still null, create a new instance
            if (s_Instance == null)
            {
//				Debug.Log("Could not locate an BrickHolder object!");
//				Debug.Break();
            }

            return s_Instance;
        }
    }

    void OnApplicationQuit()
    {
        s_Instance = null;
    }
}