
using UnityEngine;
using UnityEngine.UIElements;

public class AnswerCard : VisualElement
{
    public new class UxmlFactory : UxmlFactory<AnswerCard, UxmlTraits> { }
    
    public bool _isCorrect;
    
    
    public AnswerCard()
    {
    }
    
    
    public void Init(float sizeRate, Sprite sprite)
    {
        SetHeight(sizeRate);
        SetSprite(sprite);
    }
    
    public void SetHeight(float rate)
    {
        style.width = resolvedStyle.height * rate;
    }
    
    public void SetSprite(Sprite sprite)
    {
        style.backgroundImage = new StyleBackground(sprite);
    }
}