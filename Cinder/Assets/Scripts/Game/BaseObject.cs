using System.Collections;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    protected void PlaySound(SoundList sound)
    {
        CoreConnector.SoundManager.PlaySound(sound);
    }

    protected static void ShowInGameMessage(string _messageText)
    {
//		Debug.Log("ShowInGameMessage:"+_messageText);
        CoreConnector.GameUIManager.gameMessages.DisplayInGameMessage(_messageText);
    }

    protected static void SpawnParticles(ParticleTypes particleType, Vector3 position)
    {
        CoreConnector.GameManager.particleManager.SpawnParticleEffect(particleType, position);
    }

    public virtual void LevelComplete()
    {
    }

    public virtual void LifeLost()
    {
    }

    public virtual void BrickWasDestroyed()
    {
// this is used for after a brick is destroyed, in case bricks or other objects need to respond
    }

    protected void StopRunningCoroutine(IEnumerator coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}