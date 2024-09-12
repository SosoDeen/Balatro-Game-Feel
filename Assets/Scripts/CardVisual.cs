using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardVisual : MonoBehaviour
{
    // references
    Card matchingCard;
    Transform cardTransform;
    Canvas visualCanvas;

    // shadow references
    Transform shadow;
    Canvas shadowCanvas;

    // states
    bool isDragging = false;

    // move params
    float followSpeed = 10f;
    float shadowOffset = -10f;

    // rotation params
    Vector3 targetRotation;
    float rotationMax = 70f;
    float rotationSpeed = 20f;

    // tweening values
    [Header("Punch Tween Values")]
    float punchStrength;
    public Vector3 punchVector;
    public float punchDuration;

    [Header("Hover Tween Values")]
    public float hoverScale;
    public float hoverDuration;

    [Header("Drag Tween Values")]
    public float dragScale;

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
        hoverDuration = 0.2f;
        hoverScale = 1.2f;
        dragScale = 1.3f;
        punchStrength = 4.5f;
        punchVector = new Vector3(0, 0, punchStrength);
        punchDuration = 0.1f;

        visualCanvas = GetComponent<Canvas>();
        shadowCanvas = GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        // move card
        MoveCard();
        // rotate card as it gets dragged
        //RotateCard();
        // idle rotation on select
    }

    public void MoveCard()
    {
        //validation
        if (matchingCard == null) return;

        // move the card visual to matching card position
        transform.position = Vector3.Lerp(transform.position, cardTransform.position, followSpeed * Time.deltaTime);
    }

    // no work :<
    public void RotateCard()
    {
        // validation (might need a dragging check)
        if (matchingCard == null || !isDragging) return;

        // sohcahtoa time >:)
        float forwardMovment = transform.position.x - cardTransform.position.x; // adj
        float upMovment = transform.position.y - cardTransform.position.y; // opp
        float angle = Mathf.Atan(upMovment/forwardMovment);

        // lerp angle and set it as current rotation
        Vector3 angleVector = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, angle);
        targetRotation = Vector3.Lerp(transform.eulerAngles, angleVector, rotationSpeed * Time.deltaTime);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(targetRotation.z, -rotationMax, rotationMax));
    }

    // swap method to move the 

    // select method
    public void select()
    {

    }

    // -------------- EVENT FUNCTIONS ---------------

    public void PointerEnter()
    {
        // only do actions on hover
        if(isDragging) return;

        // shake the card on hover
        transform.DOPunchRotation(punchVector, punchDuration);

        // scale up the card on hover start
        transform.DOScale(hoverScale, hoverDuration);
    }
    public void PointerExit()
    {
        if(isDragging) return;

        // scale down the card on hover end
        transform.DOScale(1, hoverDuration);
    }
    public void PointerDown()
    {

    }
    public void PointerUp()
    {
        transform.DOPunchRotation(punchVector, punchDuration);
    }
    public void BeginDrag()
    {
        isDragging = true;

        // scale up the card on drag start
        transform.DOScale(dragScale, hoverDuration);
        // turn on sorting override to put card's sort order to top
        visualCanvas.overrideSorting = true;

        // update shadow offset
        shadow.localPosition += new Vector3(0, shadowOffset, 0);
    }
    public void EndDrag()
    {
        isDragging = false;

        // scale down the card and reset rotation on drag end
        transform.DOScale(1, hoverDuration);
        transform.rotation = Quaternion.identity;
        // turn off sorting override to return to original sort order
        visualCanvas.overrideSorting = false;

        // update shadow offset
        shadow.localPosition -= new Vector3(0, shadowOffset, 0);
    }
}
