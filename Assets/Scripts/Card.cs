using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, 
    IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    //oki here we go trying events
    public UnityEvent<Card> OnPointerDownEvent;
    public UnityEvent<Card> OnPointerUpEvent;
    public UnityEvent<Card> OnBeginDragEvent;
    public UnityEvent<Card> OnEndDragEvent;

    // states
    bool selected;

    // movement
    int cardSwapDistance; // min distance the card needs to cross before swapping hand position
    int cardOffset; // distance the card is lifted when seleced

    static bool cardswapped = false;

    // Start is called before the first frame update
    void Start()
    {
        selected = false;
        cardSwapDistance = 100;
        cardOffset = 10;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void swapCardPosition(int flipDirection)
    {
        // if the direction is negative, flip with before sibling.
        // else flip with the after sibling

        // figure out what the index of the sibling we want to go to
        int siblingIndex = transform.parent.GetSiblingIndex() + flipDirection;
        // index validation
        if (siblingIndex < 0 || siblingIndex >= transform.parent.parent.childCount) return;

        // keep a record of the card position before swap
        Vector2 cardPosition = transform.position;

        // flip the transforms of the card holders too
        Transform siblingTransform = transform.parent.parent.GetChild(siblingIndex);
        Vector3 temp = transform.parent.localPosition;

        transform.parent.localPosition = siblingTransform.localPosition;
        siblingTransform.localPosition = temp;

        // flip the hierarchy of the two card holders
        transform.parent.SetSiblingIndex(siblingIndex);

        // update card position
        transform.position = cardPosition;

        cardswapped = true;
    }

    // EVENT TRIGGERS
    public void OnDrag(PointerEventData eventData)
    {
        if(cardswapped)
        {
            int foo = 4;
            ++foo;
        }

        transform.position = eventData.position;

        if (transform.localPosition.x > (cardSwapDistance)) swapCardPosition(1);
        else if (transform.localPosition.x < (-cardSwapDistance)) swapCardPosition(-1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        selected = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = Vector3.zero;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

    }

    public void OnPointerExit(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("Selection: "+selected);
        selected = !selected;

        if (selected) transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + cardOffset);
        else transform.localPosition = Vector2.zero;
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
