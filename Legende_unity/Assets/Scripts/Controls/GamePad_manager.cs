﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fluent;

public class GamePad_manager : MonoBehaviour
{
    public static GamePad_manager instance;

    float right_stick_x;
    float right_stick_y;
    float left_stick_x;
    float left_stick_y;

    public enum game_pad_attribution{
        player,
        inventory,
        kart,
        dialogue,
        actionDisplay
    }
    public game_pad_attribution _game_pad_attribution = game_pad_attribution.player;
    public game_pad_attribution _last_game_pad_attribution = game_pad_attribution.player;

    bool currently_navigate_in_inventory;

    bool gameIsPaused;
    GameObject interaction;

    void Start()
    {
        if(instance == null){
            instance = this;
        }
    }

    public void open_close_inventory(bool is_open){
        Debug.Log("open_close_inventory " + is_open);
        if(is_open){
            if(_game_pad_attribution != game_pad_attribution.inventory){
                _last_game_pad_attribution = _game_pad_attribution;
                Time.timeScale = 0; // gamePaused
            }
            _game_pad_attribution = game_pad_attribution.inventory;
        }else{
            _game_pad_attribution = _last_game_pad_attribution;
            Time.timeScale = 1; // Back to game
             
        }
    }


    void Update()
    {
        if(hinput.anyGamepad.back.justPressed){
            inventory_navigation.instance.navigateInMainMenus(0);
        }

        right_stick_x = hinput.anyGamepad.rightStick.position.x;
        right_stick_y = hinput.anyGamepad.rightStick.position.y;

        left_stick_x = hinput.anyGamepad.leftStick.position.x;
        left_stick_y = hinput.anyGamepad.leftStick.position.y;

        switch(_game_pad_attribution){

            case game_pad_attribution.player : case game_pad_attribution.actionDisplay :

                player_gamePad_manager.instance.player_is_moving = left_stick_x < 0 || left_stick_x > 0 || left_stick_y < 0 || left_stick_y > 0;
              
                player_gamePad_manager.instance.player_velocity_calculation();

                // Gestion du mouvement du player
                if(player_gamePad_manager.instance.player_is_moving){ 
                    player_gamePad_manager.instance.player_movement(left_stick_x, left_stick_y);
                }

                // Met la camera derriere le joueur
               if(hinput.anyGamepad.leftTrigger.justPressed){
                   player_gamePad_manager.instance.put_camera_behind_player();
                   lockTarget.instance.detection_collider.enabled = true;
                }

                // lock Target
                if(hinput.anyGamepad.leftTrigger.pressed){
                    lockTarget.instance.PlayerFaceToTarget();
                    lockTarget.instance.ChangeTarget();
                }

                //Unlock Target
                if(hinput.anyGamepad.leftTrigger.released){
                    lockTarget.instance.EndTargetLock();
                }

                // Gestion du saut du player
                if(hinput.anyGamepad.Y.justPressed){
                    player_gamePad_manager.instance.player_jump();
                }

                // Attack Player
                if(hinput.anyGamepad.B.justPressed){ // B
                    player_gamePad_manager.instance.player_attack();
                }

                if(hinput.anyGamepad.A.justPressed){
                    if(_game_pad_attribution == game_pad_attribution.actionDisplay){
                        player_actions.instance.do_action();
                    }
                }

                

                // Utilise des shortcuts
                if(hinput.anyGamepad.dPad.up.justPressed){
                    inventory_shortcuts.instance.use_shortcut(0);
                }else if(hinput.anyGamepad.dPad.right.justPressed){
                    inventory_shortcuts.instance.use_shortcut(1);
                }else if(hinput.anyGamepad.dPad.down.justPressed){
                    inventory_shortcuts.instance.use_shortcut(2);
                }else if(hinput.anyGamepad.dPad.left.justPressed){
                    inventory_shortcuts.instance.use_shortcut(3);
                }

            break;


            case game_pad_attribution.inventory :
                if(hinput.anyGamepad.A.justPressed){
                    Debug.Log("Test A");
                }

                // Navigue dans les menus principaux
                if(hinput.anyGamepad.leftTrigger.justPressed){
                    inventory_navigation.instance.navigateInMainMenus(-1);
                }else if(hinput.anyGamepad.rightTrigger.justPressed){
                    inventory_navigation.instance.navigateInMainMenus(1);
                }

                // Navigue dans les slots
                if(!currently_navigate_in_inventory){
                    if(hinput.anyGamepad.leftStick.up){
                        StartCoroutine(navigate_in_inventory("up"));
                    }else if(hinput.anyGamepad.leftStick.right){
                        StartCoroutine(navigate_in_inventory("right"));
                    }else if(hinput.anyGamepad.leftStick.down){
                        StartCoroutine(navigate_in_inventory("down"));
                    }else if(hinput.anyGamepad.leftStick.left){
                        StartCoroutine(navigate_in_inventory("left"));
                    } 
                }

                // Creer des shortcuts
                // if(hinput.anyGamepad.dPad.up.justPressed){
                //     inventory_navigation.instance.go_to_shortcut(0);
                // }else if(hinput.anyGamepad.dPad.right.justPressed){
                //     inventory_navigation.instance.go_to_shortcut(1);
                // }else if(hinput.anyGamepad.dPad.down.justPressed){
                //     inventory_navigation.instance.go_to_shortcut(2);
                // }else if(hinput.anyGamepad.dPad.left.justPressed){
                //     inventory_navigation.instance.go_to_shortcut(3);
                // }


                // Actions sur les objets
                if(hinput.anyGamepad.A.justPressed){
                    inventory_navigation.instance.action_A();
                }
                if(hinput.anyGamepad.Y.justPressed){
                    inventory_navigation.instance.action_Y();
                }
                if(hinput.anyGamepad.B.justPressed){
                    inventory_navigation.instance.action_Back();
                }
            break;


            case game_pad_attribution.kart :
                    // Gere le mouvement de camera et la rotation du siege
                    if(right_stick_x != 0 || right_stick_y != 0){ 
                        kart_manager.instance.kart_movement(right_stick_x, right_stick_y, left_stick_x, left_stick_y);
                    }

                    // GERE LE FREINAGE DU VEHICULE
                    kart_manager.instance.frein(hinput.anyGamepad.leftTrigger.pressed);

                    // Gestion de la vitesse basique avec le joystick
                    kart_manager.instance.calcul_vitesse_basique(left_stick_y);

                    // Gestion du boost // Fonctionne seulement s'il y a encore de la vapeur
                    if(hinput.anyGamepad.rightTrigger.pressed && left_stick_y != 0){
                        kart_manager.instance.boost(true);
                    }
                    if(hinput.anyGamepad.rightTrigger.released || left_stick_y == 0){
                        kart_manager.instance.boost(false);
                    }

                    // Gestion du saut du kart
                    if(hinput.anyGamepad.Y.justPressed){ 
                        kart_manager.instance.kart_jump();
                    }

                    // Gestion attaque du kart
                    if(hinput.anyGamepad.B.justPressed){ 
                        kart_manager.instance.kart_attaque();
                    }

                    // Allume lumiere du kart
                    if(hinput.anyGamepad.X.justPressed){ 
                        kart_manager.instance.kart_light();
                    }

                    if(hinput.anyGamepad.A.justPressed){
                        if(_game_pad_attribution == game_pad_attribution.actionDisplay){
                            player_actions.instance.do_action();
                            Debug.Log("Bouton A do action");
                        }
                    }
            break;


            case game_pad_attribution.dialogue :
            break;

        }

    }


    IEnumerator navigate_in_inventory(string direction){

        currently_navigate_in_inventory = true;
        inventory_navigation.instance.navigate_in_slots(direction);
        yield return new WaitForSecondsRealtime(0.2f);
        currently_navigate_in_inventory = false;
    }


}
