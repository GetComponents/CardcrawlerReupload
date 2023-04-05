using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardImageTMP : MonoBehaviour
{
    public void Click()
    {
        if (SelectionManager.Instance.waitingForCardSelection)
        {
            Debug.Log("Card Image was clicked");
        }
    }
}
