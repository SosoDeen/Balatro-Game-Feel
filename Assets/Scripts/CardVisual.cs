using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    // references
    Card matchingCard;
    Transform cardTransform;
    Canvas visualCanvas;
    Material material;

    // shadow references
    Transform shadow;
    Canvas shadowCanvas;

    // states
    bool isDragging = false;

    // move params
    float followSpeed = 10f;
    float shadowOffset = -10f;

    // rotation params
    Vector3 leftoverRotation;
    Vector3 leftoverDistance;
    float rotationMax = 70f;
    float rotationSpeed = 20f;

    // tilting params
    float tiltSpeed = 5f;
    float tiltAmount = 30f;

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

        transform.position = new Vector3(cardTransform.position.x, cardTransform.position.y, 400);
        shadow = transform.GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        hoverDuration = 0.2f;
        hoverScale = 1.1f;
        dragScale = 1.3f;
        punchStrength = 4.5f;
        punchVector = new Vector3(0, 0, punchStrength);
        punchDuration = 0.1f;

        visualCanvas = GetComponent<Canvas>();
        shadowCanvas = GetComponent<Canvas>();
        material = transform.GetChild(1).GetComponent<Image>().material;
    }

    // Update is called once per frame
    void Update()
    {
        // move card
        MoveCard();
        // rotate card as it gets dragged
        RotateCard();
        // idle rotation on select
        TiltCard();
    }

    public void MoveCard()
    {
        //validation
        if (matchingCard == null) return;

        Vector3 offsetCardTransform = new Vector3(cardTransform.position.x, cardTransform.position.y, 400);

        // move the card visual to matching card position
        transform.position = Vector3.Lerp(transform.position, offsetCardTransform, followSpeed * Time.deltaTime);
    }

    // lowkey work :>
    public void RotateCard()
    {
        // validation (might need a dragging check)
        if (matchingCard == null || !isDragging) return;

        /*// sohcahtoa time >:)
        float forwardMovment = transform.position.x - cardTransform.position.x; // adj
        float upMovment = transform.position.y - cardTransform.position.y; // opp
        float angle = Mathf.Atan(upMovment/forwardMovment);
        */


        // rotation based on distance from mouse
        Vector3 distance = transform.position - cardTransform.position;
        // lerping a part of the desired distance for smooth rotation
        leftoverDistance = Vector3.Lerp(leftoverDistance, distance, Time.deltaTime);

        // how much the card will be rotated
        //Vector3 angleVector = (isDragging? leftoverDistance : distance) * rotationSpeed;
        Vector3 angleVector = distance;
        // lerping a part of the desired rotation
        leftoverRotation = Vector3.Lerp(transform.eulerAngles, angleVector, rotationSpeed * Time.deltaTime);
        // rotating the card one lerp portion at a time
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(leftoverRotation.x, -rotationMax, rotationMax));
        //Debug.Log("Distance: " + distance+ "\nAngle: " + angleVector +"\nRotation: " + leftoverRotation+ "\nFinal Rotation: " + transform.eulerAngles);
    }

    public void TiltCard()
    {
        // working backwards time!

        // for idle tile: Cosine and Sine!
        float sine = Mathf.Sin(Time.time);
        float cosine = Mathf.Cos(Time.time);
        //Debug.Log("Sine: " + sine+"  Cosine: "+cosine+"  Time: "+Time.time);

        // before we lerp on hover though, we must know where we are tilting
        float tiltX = 10;
        float tiltY = 20;
        float tiltZ = 30;

        // lerp floats to make tilt vector
        float lerpX = Mathf.LerpAngle(transform.eulerAngles.x, (sine * tiltAmount), tiltSpeed * Time.deltaTime);
        float lerpY = Mathf.LerpAngle(transform.eulerAngles.y, (cosine * tiltAmount), tiltSpeed * Time.deltaTime); ;
        float lerpZ = 0;
        // silly me made a rave card dance b/c i didnt use LerpAngle

        // first we need to set the final tilt based on lerped data
        transform.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);

        // change the angle of the shader?
        material.SetVector("_Rotation", new Vector2(sine, cosine));
    }

    // swap method to move the 

    // select method
    public void select()
    {

    }

    // -------------- EVENT FUNCTIONS ---------------

    public void Swap()
    {
        transform.DOPunchRotation(punchVector, punchDuration);
    }

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
