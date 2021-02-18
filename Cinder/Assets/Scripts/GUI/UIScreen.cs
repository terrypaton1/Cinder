using UnityEngine;

public class UIScreen : MonoBehaviour
{
    [SerializeField]
    protected GameObject content;

    public void Show()
    {
        content.SetActive(true);
    }

    public void Hide()
    {
        content.SetActive(false);
    }
}