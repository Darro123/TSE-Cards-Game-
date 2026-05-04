using UnityEngine;

public class CardClick : MonoBehaviour
{
    public Card card;

    private void OnMouseDown()
    {
        UISelection.Instance.SelectCard(card);
    }
}