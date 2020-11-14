using UnityEngine;

public class BaseObject : MonoBehaviour
{
    protected virtual void PlaySound(SoundList sound)
    {
        Messenger<SoundList>.Broadcast(GlobalEvents.PlaySoundFX, sound, MessengerMode.DONT_REQUIRE_LISTENER);
    }

    protected void ShowInGameMessage(string _messageText)
    {
//		Debug.Log("ShowInGameMessage:"+_messageText);
        Messenger<string>.Broadcast(GlobalEvents.DisplayInGameMessage, _messageText);
    }

    protected void Log(string _text)
    {
        Messenger<string>.Broadcast(GlobalEvents.DebugString, _text, MessengerMode.DONT_REQUIRE_LISTENER);
    }
}