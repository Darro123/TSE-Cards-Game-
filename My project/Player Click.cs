using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClick : MonoBehaviour
{
    public GameObject playerObject;

    private void OnMouseDown()
    {
        UISelection.Instance.SelectTarget(playerObject);
        GameLogUI.Instance.AddMessage("target player: " + playerObject.name);
    }
}