using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Controller : MonoBehaviour, Interactable
{
    [SerializeField] private Dialog dialog;

    public void Interact()
    {
        StartCoroutine(Dialog_Manager.Instance.ShowDialog(dialog));
    }
}