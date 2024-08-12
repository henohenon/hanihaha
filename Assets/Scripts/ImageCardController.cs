using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ImageCardController : MonoBehaviour, ISelectable
{
    private SpriteRenderer _spriteRenderer;
    
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    public void OnHover()
    {
        _spriteRenderer.color = Color.gray;
    }

    public void OnSelect()
    {
        _spriteRenderer.color = Color.red;
    }

    public void OnUnhover()
    {
        _spriteRenderer.color = Color.white;
    }
}
