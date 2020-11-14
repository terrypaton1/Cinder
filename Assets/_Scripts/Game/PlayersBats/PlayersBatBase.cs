using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;
using System.IO;
using System;

// The GameObject requires a Rigidbody component
#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(Rigidbody2D)), RequireComponent(typeof(Animator)), SelectionBase]
public class PlayersBatBase : BaseObject {

	[SerializeField]
protected	Animator MorphToPlayingAnimation;
	/// <summary>
	/// The rigid reference.
	/// </summary>
	[HideInInspector]
	public Rigidbody2D rigidRef;
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start() {
		rigidRef = GetComponent<Rigidbody2D>();
	}

	/// <summary>
	/// Raises the collision enter2D event.
	/// </summary>
	/// <param name="coll">Coll.</param>
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Ball")) {
//			Debug.Log("ball hit bat");	
			Messenger<ParticleTypes,Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.BallHitsBat, collision.contacts [0].point, MessengerMode.DONT_REQUIRE_LISTENER);	
		}
	}

	/// <summary>
	/// Morphs the bat to its playing state. 0-1 percent
	/// All bats (except normal) start looking normal, then are transitioned to their state. 
	/// </summary>
	/// <param name="percent">Percent.</param>
	virtual	public  void MorphToPlayState() {
		// move the physics pieces
		Debug.Log("MorphToPlayState"  );
	}
	virtual	public  void MorphToNormal() {
		// move the physics pieces
		Debug.Log("MorphToNormal"  );
	} 
	virtual	public  void PlayerLosesLife() {
		// move the physics pieces
		Debug.Log("PlayerLosesLife"  );
	}
}