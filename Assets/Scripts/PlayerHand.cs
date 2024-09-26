using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHand : MonoBehaviour
{
    public int offsetY = 10;

    public void UpdateHandArc()
    {
        // validation
        if (transform.childCount == 0) return;

        int childCount = transform.childCount - 1;
        float middleIndex = childCount / 2;

        // goes through children and offset their Y based on middle distance
        for (int i = 0; i < childCount; i++)
        {
            // get the amount of times to offset based on center
            float offsetMult = Mathf.Abs(middleIndex - i);

            // change the posision of the card
            Transform childTransform = transform.GetChild(i).GetChild(0).transform;
            Card card = childTransform.GetComponent<Card>();

            // skip selected cards
            if (card.selected) continue;

            childTransform.position = new Vector3(childTransform.position.x, 
                                                  offsetY * offsetMult, 
                                                  childTransform.position.z);
        }
    }
}
