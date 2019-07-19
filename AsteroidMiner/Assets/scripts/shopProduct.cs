﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class shopProduct : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler,IPointerExitHandler, IPointerUpHandler
{
    item i;
    bool buy;
    public Image image;
    public TextMeshProUGUI price;
    public new TextMeshProUGUI name;
    shop shop;
    public Color hover;
    public Color press;
    Image im;
    bool over;


    public void OnPointerDown(PointerEventData eventData)
    {
        im.color = press;
        shop.productStuff(i,buy);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
        im.color = hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
        Color col = im.color;
        col.a = 0;
        im.color = col;
    }

    public void setVariables(item i, bool buy){
        this.i= i;
        this.buy = buy;
        im = GetComponent<Image>();
        shop = GameObject.FindObjectOfType<shop>();
        image.sprite = i.sprite;
        if(buy){
            price.text = i.cost.ToString();
        }
        else{
            price.text = i.resellValue.ToString();
        }
        name.text = i.name;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(over)
            im.color = hover;
    }
}
