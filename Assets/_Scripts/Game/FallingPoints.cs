using UnityEngine;

public class FallingPoints : BaseObject
{
    [SerializeField]
    public GameObject pointsDisplay10;

    [SerializeField]
    public GameObject pointsDisplay50;

    [SerializeField]
    public GameObject pointsDisplay100;

    [SerializeField]
    public GameObject pointsDisplay500;

    [SerializeField]
    public GameObject visualObjects;

    private Collider2D _collider;

    private int _pointsValue;

    private int category;

    private float currentFallingSpeed;

    private bool isFalling;

    private float maximumFallingSpeed;

    private Rigidbody2D rigid2D;

    protected void FixedUpdate()
    {
        if (isFalling)
        {
            currentFallingSpeed = Mathf.Lerp(currentFallingSpeed, maximumFallingSpeed, Time.deltaTime);
            Vector2 position = rigid2D.transform.position;
            position.y -= currentFallingSpeed;
            rigid2D.MovePosition(position);
        }
    }

    protected void OnEnable()
    {
        Messenger.AddListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.AddListener(GlobalEvents.LifeLost, LifeLost);
    }

    protected void OnDisable()
    {
        Messenger.RemoveListener(MenuEvents.LevelComplete, LevelComplete);
        Messenger.RemoveListener(GlobalEvents.LifeLost, LifeLost);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("playersbat"))
        {
//			Debug.Log("Points Collected by Player");
            PlaySound(SoundList.PointsCollected);
            Messenger<int>.Broadcast(GlobalEvents.PointsCollected, _pointsValue, MessengerMode.DONT_REQUIRE_LISTENER);
            if (collision.contacts.Length > 0)
            {
                Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect,
                    ParticleTypes.FallingPointsCollected, collision.contacts[0].point,
                    MessengerMode.DONT_REQUIRE_LISTENER);
            }

            Disable();
        }

        if (collision.gameObject.CompareTag("deadzone"))
        {
            Disable();
        }
    }

    private void LifeLost()
    {
        if (!isFalling)
            return;
        Messenger<ParticleTypes, Vector3>.Broadcast(GlobalEvents.SpawnParticleEffect, ParticleTypes.DestroyFallingItems,
            transform.position, MessengerMode.DONT_REQUIRE_LISTENER);
        Disable();
    }

    private void LevelComplete()
    {
    }

    public void Setup(int newPointsValue, int _category)
    {
        category = _category;
        maximumFallingSpeed = GameVariables.maximumFallingItemSpeed;
        _pointsValue = newPointsValue;
        // disable all pointsDisplay
        HideAllVisualObjects();
        SetupVisualDisplay();
//		Debug.Log("setup falling points:" + _pointsValue);
        _collider = GetComponentInChildren<Collider2D>();
        rigid2D = GetComponent<Rigidbody2D>();
//		Debug.Log("colliders:" + colliders.Length);
        if (_collider == null)
        {
            Debug.LogError("Didn't find a colliders for falling points");
        }

        currentFallingSpeed = 0;
        isFalling = false;
    }

    private void SetupVisualDisplay()
    {
        switch (category)
        {
            default:
            case 0:
                pointsDisplay10.SetActive(true);
                break;
            case 1:
                pointsDisplay50.SetActive(true);
                break;
            case 2:
                pointsDisplay100.SetActive(true);
                break;
            case 3:
                pointsDisplay500.SetActive(true);
                break;
        }
    }

    public void StartFalling(Vector3 position)
    {
        // start falling, slowly at first, then faster
        //		Debug.Log("start falling");
        SetupVisualDisplay();
        transform.position = position;
        _collider.enabled = true;
        currentFallingSpeed = 0;
        isFalling = true;
    }

    public void Disable()
    {
//		Debug.Log("DisableFallingPoint");
        HideAllVisualObjects();
        _collider.enabled = false;
        isFalling = false;
        currentFallingSpeed = 0;
    }

    private void HideAllVisualObjects()
    {
        pointsDisplay10.SetActive(false);
        pointsDisplay50.SetActive(false);
        pointsDisplay100.SetActive(false);
        pointsDisplay500.SetActive(false);
    }
}