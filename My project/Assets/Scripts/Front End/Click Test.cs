using UnityEngine;

public class ClickTest : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("SCRIPT IS RUNNING");
    }

    private void OnMouseDown()
    {
        Debug.Log("CLICK WORKS");
    }
}