using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardVisual : MonoBehaviour
{
    // references
    Card matchingCard;
    Transform cardTransform;
    Transform shadow;

    // move params
    float followSpeed = 10f;
    float shadowOffset = -10f;

    public void Initialize(Card card)
    {
        matchingCard = card;
        cardTransform = card.transform;

        // events for DoTween
        // adding a listener:
        // script object with event . event name . add listener (function to run)
        /* No work :<
        matchingCard.OnPointerEnterEvent.AddListener(PointerEnter);
        matchingCard.OnPointerExitEvent.AddListener(PointerExit);
        matchingCard.OnPointerDownEvent.AddListener(PointerDown);
        matchingCard.OnPointerUpEvent.AddListener(PointerUp);
        matchingCard.OnBeginDragEvent.AddListener(BeginDrag);
        matchingCard.OnEndDragEvent.AddListener(EndDrag);
        */

        transform.position = cardTransform.position;
        shadow = transform.GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // move card
        MoveCard();
        // rotate card
        RotateCard();
    }

    public void MoveCard()
    {
        //validation
        if (matchingCard == null) return;

        // move the card visual to matching card position
        transform.position = Vector3.Lerp(transform.position, cardTransform.position, followSpeed * Time.deltaTime);

        // jank shadow offset
        if (transform.position.Equals(cardTransform.position))
        {
            shadow.position = transform.position;
        }
        else
        {
            shadow.position = new Vector2(transform.position.x, transform.position.y + shadowOffset);
        }
    }

    public void RotateCard()
    {
        // validation
        if (matchingCard == null) return;
    }

    // swap method to move the 

    // -------------- EVENT FUNCTIONS ---------------

    public void PointerEnter()
    {
       // shake the card on hover
       // scale up the card on hover end
    }
    public void PointerExit()
    {
        // scale down the card on hover end
    }
    public void PointerDown()
    {

    }
    public void PointerUp()
    {

    }
    public void BeginDrag()
    {
        // scale up the card on drag start
        // turn on sorting override to put card's sort order to top
    }
    public void EndDrag()
    {
        // scale down the card on drag end
        // turn off sorting override to return to original sort order
    }
}
