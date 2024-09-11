using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Card : MonoBehaviour, 
    IDragHandler, IBeginDragHandler, IEndDragHandler,
    IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler, IPointerDownHandler
{
    // card visual
    public CardVisual cardVisual;
    public GameObject cardVisualPrefab;
    public VisualsHandler visualsHandler;

    // states
    bool selected;
    bool wasDragged;

    // movement
    int cardSwapDistance; // min distance the card needs to cross before swapping hand position
    int cardOffset; // distance the card is lifted when seleced


    //oki here we go trying events
    public UnityEvent<Card> OnPointerEnterEvent;
    public UnityEvent<Card> OnPointerExitEvent;
    public UnityEvent<Card> OnPointerDownEvent;
    public UnityEvent<Card> OnPointerUpEvent;
    public UnityEvent<Card> OnBeginDragEvent;
    public UnityEvent<Card> OnEndDragEvent;

    // ------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        selected = false;
        cardSwapDistance = 100;
        cardOffset = 10;


        visualsHandler = FindObjectOfType<VisualsHandler>();
        // make a visual clone of this card
        cardVisual = Instantiate(cardVisualPrefab, visualsHandler.transform).GetComponent<CardVisual>();
        cardVisual.Initialize(this);
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
    }


    // ----------- EVENT HANDLERS (IN ORDER OF TRIGGER) -----------
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Order Test: Pointer Enter"); 
    }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Order Test: Pointer Down");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // set was dragged to maintain selection status
        wasDragged = true;

        Debug.Log("Order Test: Drag Begin");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;

        if (transform.localPosition.x > (cardSwapDistance)) swapCardPosition(1);
        else if (transform.localPosition.x < (-cardSwapDistance)) swapCardPosition(-1);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // dont change selection type if card was being dragged
        if (wasDragged) return;

        //Debug.Log("Selection: "+selected);
        selected = !selected;

        if (selected) transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + cardOffset);
        else transform.localPosition = Vector2.zero;

        Debug.Log("Order Test: Pointer Up");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // reset local pos of card to return to holder center
        transform.localPosition = Vector3.zero;
        // clear dragged state to allow card selection change
        wasDragged = false;

        Debug.Log("Order Test: Drag End");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Order Test: Pointer Exit");
    }
}
