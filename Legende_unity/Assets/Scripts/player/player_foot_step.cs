﻿using UnityEngine;
using System.Collections;
  
public class player_foot_step: MonoBehaviour{


    public CharacterController controller;

    IEnumerator Start() {

        while(true){

            RaycastHit hit = new RaycastHit();
        
            if(controller.isGrounded == true){

                if(Physics.Raycast(transform.position, Vector3.down,out hit, 1f)){

                    Player_sound.instance.TypeSol =  hit.collider.tag;       
                }  
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.02f);    
        }
    }


    public void tets(){ // test anim coup d'epee
        

        Player_sound.instance.PlayFightFx(gameObject,Player_sound.instance.FightFx[0]);

    }

}