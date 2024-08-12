using UnityEngine;

public class ImageCardController : MonoBehaviour, ISelectable
{
    public void OnHover()
    {
        Debug.Log("OnHover");
    }

    public void OnSelect()
    {
        Debug.Log("OnSelect");
    }

    public void OnUnhover()
    {
        Debug.Log("OnUnhover");
    }
}
