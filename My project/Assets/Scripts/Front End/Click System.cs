using UnityEngine;

public class CardClick : MonoBehaviour
{
    public Card card;

    private void OnMouseDown()
    {
        if (UISelection.Instance.selectedCard == null)
        {
            UISelection.Instance.selectedCard = card;
        }
        else
        {
            UISelection.Instance.SelectTarget(gameObject);
        }

    }
    
    
}