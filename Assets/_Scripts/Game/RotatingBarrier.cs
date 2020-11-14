using UnityEngine;
using System.Collections;

public class RotatingBarrier : MonoBehaviour
{
	public bool rotateClockwise = true;
	public float speed = 45f;
	Rigidbody2D _rigidbody2D;

	void Start ()
	{
		_rigidbody2D = GetComponent<Rigidbody2D> ();
	}

	void FixedUpdate ()
	{
		if (rotateClockwise) {
			_rigidbody2D.MoveRotation (_rigidbody2D.rotation + speed * Time.fixedDeltaTime);
		} else {
			_rigidbody2D.MoveRotation (_rigidbody2D.rotation- speed * Time.fixedDeltaTime);
		}        
	}
}
