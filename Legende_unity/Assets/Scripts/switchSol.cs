﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchSol : MonoBehaviour
{


  Animator anim;
  public bool switchSolIsPressed;
  public bool MultiPush; // comportemet du switch

   public GameObject Wagon; 
   Animator animWagon;
   bool AllerRetour;

   void Start(){

        anim = GetComponent<Animator>();
        switchSolIsPressed = false;
        AllerRetour = false;
       

        
        if (Wagon != null){
          animWagon = Wagon.GetComponent<Animator>();
        }  
    } 

    void OnTriggerEnter(){

        anim.SetBool("switchSol", true);
        
    }

    void OnTriggerExit(){

      if(MultiPush == true){

          if(!switchSolIsPressed){
            
             switchSolIsPressed = false;  
          }else{
              anim.SetBool("switchSol", false); 
              switchSolIsPressed = false;  
          }
      }

     
    }


    void SwitchPressed(){  // on attend que le switch s'enfonce completement en fin d'anim

        switchSolIsPressed = true;

         if (Wagon != null){
         AllerRetour = !AllerRetour;
         animWagon.SetBool("startWagon",AllerRetour);
          
      }  
    }

    

}
