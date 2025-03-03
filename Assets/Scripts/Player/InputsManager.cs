using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputsManager : MonoBehaviour
{
    public InputActionAsset actions;

    private InputAction move;
    private InputAction jump;
    private InputAction attack;

    private const string ACTION_MAP = "Player";
    private const string MOVE = "Move";
    private const string JUMP = "Jump";
    private const string ATTACK = "Attack";

    private void Awake()
    {
        move = actions.FindActionMap(ACTION_MAP).FindAction(MOVE);
        jump = actions.FindActionMap(ACTION_MAP).FindAction(JUMP);
        attack = actions.FindActionMap(ACTION_MAP).FindAction(ATTACK);
    }

    private void OnEnable()
    {
        actions.FindActionMap(ACTION_MAP).Enable();
    }

    private void OnDisable()
    {
        actions.FindActionMap(ACTION_MAP).Disable();
    }

    public float ReturnMoveValue()
    {
        return move.ReadValue<float>();
    }
}
