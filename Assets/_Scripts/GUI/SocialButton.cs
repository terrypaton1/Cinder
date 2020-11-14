#region

using UnityEngine;

#endregion

public class SocialButton : MonoBehaviour {
	[SerializeField]
	UISprite _sprite;

	[SerializeField]
	BoxCollider _collider;

	void Update() {
		if (GameVariables.instance.playerIsLoggedIn) {
			_sprite.alpha = 1;
			_collider.enabled = true;
		} else {
			_sprite.alpha = .3f;
			_collider.enabled = false;
		}
	}
}
