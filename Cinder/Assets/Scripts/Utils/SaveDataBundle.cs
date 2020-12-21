using UnityEngine;
using System;

[Serializable]
public class SaveDataBundle
{
    public int m_currentLevelPlayerIsUpto;
    public int m_bestScore;
    public int m_hasPlayerAlreadyDonated;
    public int m_totalBricksDestroyed;
    public int m_SFXEnabled = 1;

    public SaveDataBundle(int initialLevelPlayerIsUpto, int playersBestScore)
    {
        m_currentLevelPlayerIsUpto = initialLevelPlayerIsUpto;
        m_bestScore = playersBestScore;
    }

    public static SaveDataBundle FromByteArray(byte[] array)
    {
        if (array == null || array.Length == 0)
        {
            Debug.LogWarning("Serialization of zero sized array!");
            return null;
        }

        using (var stream = new System.IO.MemoryStream(array))
        {
            try
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                var bundle = (SaveDataBundle) formatter.Deserialize(stream);
                return bundle;
            }
            catch (Exception e)
            {
                Debug.LogError("Error when reading stream: " + e.Message);
            }
        }

        return null;
    }

    public static byte[] ToByteArray(SaveDataBundle bundle)
    {
        var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        using (var stream = new System.IO.MemoryStream())
        {
            formatter.Serialize(stream, bundle);
            return stream.ToArray();
        }
    }
}