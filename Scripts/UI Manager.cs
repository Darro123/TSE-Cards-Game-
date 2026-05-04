using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void PlayCard()
    {
        var sel = UISelection.Instance;

        if (sel.selectedCard == null) return;

        TurnManager.Instance.PlayCardFaceDown(
            sel.selectedCard,
            sel.selectedTarget,
            false,
            sel.selectedCard.abilityType
        );

        sel.selectedCard = null;
        sel.selectedTarget = null;
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