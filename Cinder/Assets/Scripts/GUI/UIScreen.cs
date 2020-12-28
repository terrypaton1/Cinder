using UnityEngine;

public class UIScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject content;

    public void Show()
    {
        content.SetActive(true);
    }

    public void Hide()
    {
        content.SetActive(false);
    }
}