using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class shop : window
{
    public int[] products;
    public Transform productPanel;
    public GameObject prefab;
    inventory inv;
    public Button buyButton;
    public Button sellButton;
    public TextMeshProUGUI speechBubble;
    [TextArea]
    public string[] welcomes;
    [TextArea]
    public string[] buySpeech;
    [TextArea]
    public string[] sellSpeech;
    [TextArea]
    public string[] thx;
    void Start()
    {
        inv = inventory.instance;
    }
    public void switchBuy(){
        sellButton.interactable = true;
        buyButton.interactable = false;
        speechBubble.text = buySpeech[Random.Range(0,buySpeech.Length)];
        loadProducts();
    }
    public void switchSell(){
        sellButton.interactable = false;
        buyButton.interactable = true;
        speechBubble.text = sellSpeech[Random.Range(0,sellSpeech.Length)];
        loadInventory();
    }
    public override void pressE(){
        open();
        loadProducts();
        speechBubble.text = welcomes[Random.Range(0,welcomes.Length)];
        sellButton.interactable = true;
        buyButton.interactable = false;
    }
    void loadProducts(){
        foreach(Transform child in productPanel){
            Destroy(child.gameObject);
        }
        foreach(int i in products){
            GameObject g = GameObject.Instantiate(prefab,Vector2.zero,Quaternion.identity,productPanel);
            g.GetComponent<shopProduct>().setVariables(items.instance.itemObjects[i],true,1);
        }
    }
    void loadInventory(){
        Vector2Int[] invSlots = inv.GetSlots();
        foreach(Transform child in productPanel){
            Destroy(child.gameObject);
        }
        for(int i = 0; i < invSlots.Length; i++){
            GameObject g = GameObject.Instantiate(prefab,Vector2.zero,Quaternion.identity,productPanel);
            shopProduct sP = g.GetComponent<shopProduct>();
            sP.slot = i;
            sP.setVariables(items.instance.itemObjects[invSlots[i].x],false, invSlots[i].y);
            
        }
    }
    public void buy(item i,bool buy){
        bool okay = inv.AddMoney(-i.cost);
        if(!okay){
               speechBubble.text = "get some money!";
               return;
        }
        speechBubble.text = thx[Random.Range(0,thx.Length)];
        bool ok = inv.AddToSlots(items.instance.GetIndex(i));
        if(!ok){
            inv.AddMoney(i.cost);
            speechBubble.text = "your inventory is full you stupid piece of shit";
        }
    }
    public void sell(int slot,Transform product, item i, int count){
        inv.AddMoney(i.resellValue*count);
        speechBubble.text = thx[Random.Range(0,thx.Length)];
        inv.RemoveFromSlots(slot);
        Destroy(product.gameObject);
    }   
}
