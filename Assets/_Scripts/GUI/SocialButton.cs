using UnityEngine;

public class SocialButton : MonoBehaviour
{
    [SerializeField]
    protected UISprite _sprite;

    [SerializeField]
    protected BoxCollider _collider;

    protected void Update()
    {
        if (GameVariables.instance.playerIsLoggedIn)
        {
            _sprite.alpha = 1;
            _collider.enabled = true;
        }
        else
        {
            _sprite.alpha = .3f;
            _collider.enabled = false;
        }
    }
}