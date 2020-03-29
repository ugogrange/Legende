﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;



public class Inventaire : MonoBehaviour
{
    public static int cleTrouve;
    public static string InfoText;

    void Start()
    {
      cleTrouve = 0;
      InfoText = "";
    }

    

    public void compteurCle(){

        if(cleTrouve > 0){
            GameObject.Find("nbrKey").GetComponent<UnityEngine.UI.Text>().text = cleTrouve.ToString(); // maj du text
            GameObject.Find ("PanelKey").GetComponent<Animator>().SetBool("panelKeyIsOpen", true); // affichage ui clé
            GameObject.Find ("PanelInfo").GetComponent<Animator>().SetTrigger("panelInfo");
        }
        else{
           GameObject.Find ("PanelKey").GetComponent<Animator>().SetBool("panelKeyIsOpen", false); // affichage ui clé
        }
        
    }



    public void afficheInfoText(string Montext){

        GameObject.Find ("TextInfo").GetComponent<UnityEngine.UI.Text>().text = Montext;
        GameObject.Find ("PanelInfo").GetComponent<Animator>().SetTrigger("panelInfo");
        
    }


    

}