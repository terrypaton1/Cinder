using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator)), SelectionBase]
public class PlayersBatBase : BaseObject
{
    [SerializeField]
    protected Animator MorphToPlayingAnimation;

    [HideInInspector]
    public Rigidbody2D rigidRef;

    protected void Start()
    {
        rigidRef = GetComponent<Rigidbody2D>();
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
//			Debug.Log("ball hit bat");	
            Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.BallHitsBat,
                collision.contacts[0].point, MessengerMode.DONT_REQUIRE_LISTENER);
        }
    }

    virtual public void MorphToPlayState()
    {
        // move the physics pieces
        Debug.Log("MorphToPlayState");
    }

    virtual public void MorphToNormal()
    {
        // move the physics pieces
        Debug.Log("MorphToNormal");
    }

    virtual public void PlayerLosesLife()
    {
        // move the physics pieces
        Debug.Log("PlayerLosesLife");
    }
}