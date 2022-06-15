using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]
public class ButtonController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    RectTransform rect;
    Button button;

    public float scaleAmount = 1.2f;
    public float scaleSpeed = 8f;
   
    Vector3 startScale;
    bool mouseOver;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
        button = GetComponent<Button>();

        startScale = rect.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (button.interactable) //If button is unlocked
        {
            if (mouseOver) //Scale bigger
            {
                rect.localScale = Vector3.Lerp(rect.localScale, startScale * scaleAmount, scaleSpeed * Time.deltaTime);
            }       
            else //Scale to normal
            {
                rect.localScale = Vector3.Lerp(rect.localScale, startScale, scaleSpeed * Time.deltaTime);
            }             
        }   
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (button.interactable)
        {
            UISounds.sounds.PlayHoverSound();

            mouseOver = true;
        }      
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOver = false;
    }

    public void ResetMouseOver()
    {
        if (rect != null)
            rect.localScale = Vector3.one;

        mouseOver = false;
    }
}
