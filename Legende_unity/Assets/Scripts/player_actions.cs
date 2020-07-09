﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class player_actions : MonoBehaviour
{
    public static player_actions instance;
    public object currently_displayed_action;

    void Start(){ 
        
        if(instance == null){
            instance = this;
        }   
    }

   
    // On affiche l'action a faire
    public void display_actions(object action_init, Collider collider){

        if(collider.tag == "Player"){
            currently_displayed_action = action_init;

            if(action_init.GetType() == typeof(inventory_loot)){
                ButtonAction.instance.Action("Ramasser"); 
            }

             if(action_init.GetType() == typeof(inventory_object)){
                ButtonAction.instance.Action("Prendre"); 
            }


            if(action_init.GetType() == typeof(Porte)){
                ButtonAction.instance.Action("Ouvrir"); 
            }
            
            if(action_init.GetType() == typeof(Coffre)){
                ButtonAction.instance.Action("Ouvrir"); 
            }

            if(action_init.GetType() == typeof(AscenseurSwitch)){
                ButtonAction.instance.Action("Activer"); 
            }

            if(action_init.GetType() == typeof(EnterChariot)){
                ButtonAction.instance.Action("Monter A Bord");   
            }
        }
        
        else if(collider.tag == "PlayerKart"){
            currently_displayed_action = action_init;

            if(action_init.GetType() == typeof(GareKart)){
                ButtonActionKart.instance.Action("Descendre");  
            }
            if(action_init.GetType() == typeof(aiguillage_kart)){
                ButtonActionKart.instance.Action("Bifurquer");  
            }

        }

        GamePad_manager.instance._game_pad_attribution = GamePad_manager.game_pad_attribution.actionDisplay;
    }


    // on declenche l'action avec bouton A via gamepad manager
    public void do_action(){
       
        if(currently_displayed_action.GetType() == typeof(Porte)){ 
            do_action_porte((Porte)currently_displayed_action);
            clear_action(true);
        } 

        else if(currently_displayed_action.GetType() == typeof(Coffre)){ 
            do_action_coffre((Coffre)currently_displayed_action);
            clear_action(true);
        }

        else if(currently_displayed_action.GetType() == typeof(AscenseurSwitch)){ 
            do_action_ascenseur((AscenseurSwitch)currently_displayed_action);
            clear_action(true);
        }  

        else if(currently_displayed_action.GetType() == typeof(inventory_loot)){ 
            do_action_loot((inventory_loot)currently_displayed_action);
            clear_action(true);
        }

        else if(currently_displayed_action.GetType() == typeof(inventory_object)){ 
            do_action_objet((inventory_object)currently_displayed_action);
            clear_action(true);
        }

        else if(currently_displayed_action.GetType() == typeof(EnterChariot)){ 
            do_action_enter_kart((EnterChariot)currently_displayed_action);
            clear_action(false);
        }

        else if(currently_displayed_action.GetType() == typeof(aiguillage_kart)){ 
            do_action_switch_rails((aiguillage_kart)currently_displayed_action);
            clear_action(false);
        } 

       
    }

   
    public void clear_action(bool isPlayer){

        if(isPlayer){
            ButtonAction.instance.Hide(); 
            GamePad_manager.instance._game_pad_attribution = GamePad_manager.game_pad_attribution.player; // TODO attention avec kart
            currently_displayed_action = null; 
        }
        else if(!isPlayer){
           // ButtonActionKart.instance.Hide(); 
            GamePad_manager.instance._game_pad_attribution = GamePad_manager.game_pad_attribution.kart; // TODO attention avec kart
            currently_displayed_action = null; 
        }
    }



    public void do_action_enter_kart(EnterChariot _enter_kart){

        Player_sound.instance.StopMove();

        Camera_control.instance.player_kart_camera.m_XAxis.Value = 0f;// recentre la cam
        Camera_control.instance.player_kart_camera.m_YAxis.Value = 0.3f;
        Camera_control.instance.player_kart_camera.Priority = 11;

        player_gamePad_manager.instance.PlayerCanMove(false);
        _enter_kart.player_foot.SetActive(false); 
        _enter_kart.player_kart.SetActive(true);
    
        _enter_kart.ui_chariot.scaleFactor = 0.8f; // affichage ui kart todo
        _enter_kart.script_kart_manager.enabled = true; 
        _enter_kart.chariot_siege.transform.localRotation = Quaternion.Euler(270,90,-90); // on recentre le player dans le kart
    
        GamePad_manager.instance._game_pad_attribution = GamePad_manager.game_pad_attribution.kart;   
        _enter_kart.script_kart_manager.SplineFollow.IsRunning = true; 
    }



    
    // Logique aiguillage
    public void do_action_switch_rails(aiguillage_kart _aiguillage_kart){
   
        _aiguillage_kart.toggle = !_aiguillage_kart.toggle;

        if(_aiguillage_kart.toggle){
            _aiguillage_kart._choix_circuit = aiguillage_kart.ChoixCircuit.Gauche;
            AiguillageManager.instance.next_rails = _aiguillage_kart.left_rails;
            _aiguillage_kart.anim.SetBool("switch",true);
        }
        else{
            _aiguillage_kart._choix_circuit = aiguillage_kart.ChoixCircuit.Droite;
            AiguillageManager.instance.next_rails = _aiguillage_kart.right_rails;
            _aiguillage_kart.anim.SetBool("switch",false);
        }  
    }


    // Logique Loot
    public void do_action_loot(inventory_loot _inventory_loot){

       Debug.Log("Loot ramasse");
    }

    // Logique pick up objet
    public void do_action_objet(inventory_object _inventory_loot){

        Debug.Log("add " + _inventory_loot.nom + " to inventory");
        Player_sound.instance.PlayMusicEventPlayer(Player_sound.instance.MusicEventPlayer[1]); 
        _inventory_loot.addObject();
    }


    // Logique Ascenseur
    public void do_action_ascenseur(AscenseurSwitch _ascenseur_switch){

        _ascenseur_switch.toggle_levier = !_ascenseur_switch.toggle_levier;
        _ascenseur_switch.anim_levier.SetBool("active_levier",_ascenseur_switch.toggle_levier);
        _ascenseur_switch.sound.Play();

        if(_ascenseur_switch.elevator_script.isPositionUp){
            _ascenseur_switch.anim_elevator.SetBool("position_up",false);
        }
        else{
            _ascenseur_switch.anim_elevator.SetBool("position_up",true);
        } 
    }
      
    // Logique Coffre
    public void do_action_coffre(Coffre _coffre){

        if(!_coffre.isOpen){

            _coffre.isOpen = true;
          
            if(!_coffre.petit_coffre){

                _coffre.anim.SetTrigger("OpenCoffre");
                player_gamePad_manager.instance.PlayerCanMove(false);
                Player_sound.instance.PlayMusicEventPlayer(Player_sound.instance.MusicEventPlayer[0]); 
                StartCoroutine(FadeMixer.StartFade(Music_sound.instance.MusicMaster, "musicMasterVolume", 2f , 0f)); // cut zic
            }
            else{
                _coffre.anim.SetTrigger("OpenPetitCoffre");
                _coffre.sound.clip = _coffre.audio_clip[Random.Range(0,_coffre.audio_clip.Length)];
                _coffre.sound.Play();
                _coffre.Invoke("activeObject",0.5f);
            }
        }
    }

    // Logique Porte
    public void do_action_porte(Porte _porte){

        if (_porte.Switch == null){
        
            if (_porte.keysList.Where(a => a != null).Count() == 0){
                _porte.animPorte.SetBool(_porte.typeAnimation, true);
                _porte.soundFx.Play(0);
                _porte.isOpen = true;
                Inventaire.cleTrouve = 0;
                _porte.UIInventaire.compteurCle();
            }
            if(_porte.keysList.Where(a => a != null).Count() > 0){

                if (_porte.keysList.Length > 1){
                    _porte.UIInventaire.afficheInfoText("Il vous faut "+_porte.keysList.Length+" clés");
                }
                else{
                    _porte.UIInventaire.afficheInfoText("Il vous faut "+_porte.keysList.Length+" clé");
                }
            }  
        }

        if (_porte.Switch != null){

            if( _porte.SwitchScript.switchSolIsPressed == false){
                _porte.UIInventaire.afficheInfoText("Trouvez l'interrupteur !");  
            }
            if(_porte.SwitchScript.switchSolIsPressed == true){
                _porte.animPorte.SetBool(_porte.typeAnimation, true);

                if(_porte.OneShot){
                    _porte.UIInventaire.afficheInfoText("Vous avez déverrouillé la Porte !");
                    _porte.OneShot = false; 
                    _porte.isOpen = true;
                }
            
            }
        }
    }







}
