using R3;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AnswerCardController : MonoBehaviour, ISelectable
{
    
    private SpriteRenderer _spriteRenderer;
    
    private static readonly Subject<Unit> sampleStaticSubject = new();

    public Subject<bool> onHover { get; } = new ();
    public Subject<Unit> onClick { get; } = new ();
    public Subject<bool> onAnswer { get; } = new ();
    
    private Observable<Unit> OnClick => onClick;
    
    private bool _isAnswered = false;
    
    private void Start()
    {
        sampleStaticSubject.OnNext(Unit.Default);

        _spriteRenderer = GetComponent<SpriteRenderer>();

        
        onHover.Subscribe(isHovered =>
        {
            if (_isAnswered) return;
            if (isHovered)
            {
                _spriteRenderer.color = Color.gray;
            }
            else
            {
                _spriteRenderer.color = Color.white;
            }
        }).AddTo(this);
        
        onAnswer.Subscribe(isCorrect =>
        {
            if (isCorrect)
            {
                _spriteRenderer.color = new Color(14/255, 255/255, 0);
            }
            else
            {
                _spriteRenderer.color = Color.red;
            }
            _isAnswered = true;
        }).AddTo(this);
    }
    
}

public interface ISelectable
{
    public Subject<bool> onHover { get; }
    public Subject<Unit> onClick { get; }
    public Subject<bool> onAnswer { get; }
}
