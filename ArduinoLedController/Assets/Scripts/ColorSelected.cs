using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorSelected : MonoBehaviour,  IPointerDownHandler
{
    public Color color;
    public int id;
    public bool BeingPicked;

    public Image spRenderer;
    public Animator anim;

    private void Start()
    {
        spRenderer = GetComponent<Image>();
        anim = GetComponent<Animator>();
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        BeingPicked = true;
        anim.SetBool("BeingPicked", true);
    }
}
