using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState{ FreeRoam, Dialog, Battle }

public class Game_Controller : MonoBehaviour
{
    [SerializeField] Player_Controller player_Controller;

    GameState state; 

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            player_Controller.HandleUpdate();

        } else if (state == GameState.Dialog)
        {

        } else if (state == GameState.Battle)
        {

        }
    }
}
