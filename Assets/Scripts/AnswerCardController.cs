using R3;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnswerCardController : MonoBehaviour, ISelectable
{
    
    private SpriteRenderer _spriteRenderer;
    
    private static readonly Subject<Unit> sampleStaticSubject = new();

    public Subject<bool> onHover { get; } = new ();
    public Subject<Unit> onClick { get; } = new ();

    private Observable<Unit> OnClick => onClick;
    
    
    private void Start()
    {
        sampleStaticSubject.OnNext(Unit.Default);

        _spriteRenderer = GetComponent<SpriteRenderer>();

        var token = this.destroyCancellationToken;
        OnClick.Subscribe(isHovered =>
        {
        });
    }
    
}

public interface ISelectable
{
    public Subject<bool> onHover { get; }
    public Subject<Unit> onClick { get; }
}