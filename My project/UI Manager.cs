using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void PlayCard()
    {
        var selected = UISelection.Instance;

        if (selected.selectedCard == null) return;

        TurnManager.Instance.PlayCardFaceDown(selected.selectedCard, selected.selectedTarget,false, selected.selectedCard.abilityType);

        selected.selectedCard = null;
        selected.selectedTarget = null;
    }

    public void CallLiar()
    {
        TurnManager.Instance.ResolveLiarCall(true, 1);
    }

    public void Pass()
    {
        TurnManager.Instance.ResolveLiarCall(false, 0);
    }
}