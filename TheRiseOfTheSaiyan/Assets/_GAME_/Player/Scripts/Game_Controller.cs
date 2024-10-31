using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Dialog, Battle }

public class Game_Controller : MonoBehaviour
{
    [SerializeField] private Player_Controller player_Controller;
    private GameState state;

    private void Start()
    {
        if (player_Controller == null)
        {
            player_Controller = FindObjectOfType<Player_Controller>();
            if (player_Controller == null)
            {
                Debug.LogError("Player_Controller not found in the scene. Please assign it in the Inspector.");
            }
        }

        if (Dialog_Manager.Instance != null)
        {
            Dialog_Manager.Instance.OnShowDialog += () =>
            {
                state = GameState.Dialog;
            };

            Dialog_Manager.Instance.OnHideDialog += () =>
            {
                if (state == GameState.Dialog)
                    state = GameState.FreeRoam;
            };
        }
        else
        {
            Debug.LogError("Dialog_Manager instance is missing. Ensure Dialog_Manager is in the scene.");
        }
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            if (player_Controller != null)
                player_Controller.HandleUpdate();
            else
                Debug.LogError("Player_Controller reference is missing in Game_Controller.");
        }
        else if (state == GameState.Dialog)
        {
            if (Dialog_Manager.Instance != null)
                Dialog_Manager.Instance.HandleUpdate();
        }
    }
}
