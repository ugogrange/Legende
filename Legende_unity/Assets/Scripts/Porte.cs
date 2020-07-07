﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;


public class Porte : MonoBehaviour
{

 public GameObject[] keysList;

 public string typeAnimation;
 
 public typeOuverture _type_ouverture;
    public enum typeOuverture{
        classique,
        coulissant,
        slideUp,
        chute     
    }

 public Animator animPorte;
 public GameObject Switch; // on attache le switch sol voulu
 public switchSol SwitchScript; // variable pour recuperer la bool dnas le script switchSol
 public Inventaire UIInventaire; // variable pour recuperer les animations de l'UI
 public AudioSource soundFx;

 public bool OneShot; // on affiche qu'une fois "porte verrouilée"
 public bool AutoClosed;
 public int time_auto_closed = 3;
 public bool isOpen;
 public string nom_de_la_porte = "Porte du Boss";

    void Start(){

        UIInventaire = GameObject.Find("UI_Main").GetComponent<Inventaire>();
        OneShot = true;
        animPorte = GetComponentInChildren<Animator>(); 
        soundFx = GetComponent<AudioSource>();

        switch (_type_ouverture){

            case typeOuverture.classique : typeAnimation = "isOpenPivot"; break;
            case typeOuverture.coulissant : typeAnimation = "isOpenSlide"; break;
            case typeOuverture.slideUp : typeAnimation = "isOpenUp"; break;
            case typeOuverture.chute : typeAnimation = "isChute"; break;
        }
     
        if (Switch != null){
            SwitchScript = Switch.GetComponent<switchSol>(); // On recupere la bool dans le script
        }    
    } 

    void OnTriggerEnter(Collider collider){ 

        if(!isOpen){
            player_actions.instance.display_actions(this,collider); 
        }
        StopCoroutine("fermeture");
    }



    void OnTriggerExit(Collider collider){

        if(AutoClosed && collider.tag =="Player"){
            StartCoroutine("fermeture");
        }
        player_actions.instance.clear_action(collider.tag == "Player");      
    }

    IEnumerator fermeture(){

        yield return new WaitForSeconds(time_auto_closed);   
        animPorte.SetBool(typeAnimation, false);      
        isOpen = false;    
    }
    

}








