using System.Collections;
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
    public int slot;


    public void OnPointerDown(PointerEventData eventData)
    {
        im.color = press;
        if(buy){
            shop.buy(i,buy);
        }
        else{
            shop.sell(slot,transform,i,count);
        }
        
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
    int count;

    public void setVariables(item i, bool buy, int count){
        
        if(count == 0){
            Destroy(gameObject);
            return;
        }
        this.count = count;
        this.i= i;
        this.buy = buy;
        im = GetComponent<Image>();
        shop = GameObject.FindObjectOfType<shop>();
        image.sprite = i.sprite;
        if(buy){
            price.text = i.cost.ToString();
        }
        else{
            price.text = (i.resellValue*count).ToString();
        }
        if(count > 1){
             name.text = count.ToString() + "x "+i.name;
        }
        else
            name.text = i.name;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(over)
            im.color = hover;
    }
}
