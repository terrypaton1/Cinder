using UnityEngine;
using System.Collections;

public class MoveBackground : MonoBehaviour {
Vector3 position;
float rotation = 0;
float range = .2f;
	// Use this for initialization
	void Start () {
		position = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	float radian = Mathf.Deg2Rad*rotation;
		Vector3 newPosition = position;
		newPosition.x +=Mathf.Cos(radian)*range;
		newPosition.y +=Mathf.Sin(radian)*range;
			transform.position = newPosition;
		rotation+=Time.deltaTime*10;
	}
}
