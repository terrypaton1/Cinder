using System.Collections;
using System.Collections.Generic;
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

   [SerializeField]
   private BrickBase[] brickPrefabs;

   
}
