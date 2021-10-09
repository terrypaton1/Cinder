using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class WaitCache : MonoBehaviour
{
    // used for caching WaitForSeconds coroutines

    private static WaitCache _waitCache;
    private Dictionary<string, YieldInstruction> cache;

    private void Awake()
    {
        _waitCache = this;
        Debug.Log("WaitCache.Awake()\n");
        cache = new Dictionary<string, YieldInstruction>();
    }

    public static YieldInstruction WaitForSeconds(float time)
    {
        // time is rounded to limit the amount of times that could be cached,
        // eg: there's little point in caching thousands of a second
        var roundedTime = Mathf.Round(time * 100);
        var simplifiedTime = roundedTime / 100.0f;
        var simplifiedTimeKey = simplifiedTime.ToString(CultureInfo.InvariantCulture);

        if (_waitCache.cache.TryGetValue(simplifiedTimeKey, out var returnWait))
        {
            return returnWait;
        }
        else
        {
            returnWait = new WaitForSeconds(simplifiedTime);
            _waitCache.cache.Add(simplifiedTimeKey, returnWait);
        }

        return returnWait;
    }
}