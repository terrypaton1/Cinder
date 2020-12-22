using UnityEngine;

public class UIScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject content;

    public virtual void Show()
    {
        content.SetActive(true);
    }

    public virtual void Hide()
    {
        content.SetActive(false);
    }
}