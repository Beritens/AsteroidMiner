using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shop : selectable
{
    public GameObject window;
    
    public override void pressE(){
        openShop();
    }

    void openShop(){
        window.SetActive(true);
    }
}
