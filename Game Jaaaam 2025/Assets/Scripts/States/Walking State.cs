using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;

public class WalkingState : BaseState
{
    S_Player_Data playerData;
    StateType stateType = StateType.WALKING;

    StateMachine stateMachine;
    Rigidbody rb;
    InputAction moveAction;

    int direction = 2;
    private int lastDirection = 2;
    public static Vector2 playerDirection;
    private S_MapToIsland mapToIsland;
    private RuntimeAnimatorController mapAnimator;
    private RuntimeAnimatorController islandAnimator;
    private Animator spriteAnimator;
    private bool canMove = true;

    public void DisableMovement()
    {
        canMove = false;
        rb.linearVelocity = Vector3.zero; // Stop movement immediately
    }

    public void EnableMovement()
    {
        canMove = true;
    }

    public void Enter(StateMachine pa_stateMachine)
    {
        Debug.Log("entered walking state");
        playerData = Resources.Load<S_Player_Data>("Scriptable Objects/S_Player_Data");

        stateMachine = pa_stateMachine;
        moveAction = InputSystem.actions.FindAction("Move");
        rb = stateMachine.gameObject.GetComponent<Rigidbody>();

        mapToIsland = Resources.Load<S_MapToIsland>("Scriptable Objects/S_MapToIsland");
        mapAnimator = mapToIsland.mapAnimator;
        islandAnimator = mapToIsland.islandAnimator;

        spriteAnimator = stateMachine.gameObject.GetComponentInChildren<Animator>();
    }

    public void UpdateState()
    {
        if (!canMove) return;

        CheckExitConditions();

        Vector2 move = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector3(move.x * playerData.movementSpeed, 0, move.y * playerData.movementSpeed);

        if (move != Vector2.zero)
        {
            playerDirection = move; // Save the direction the player is facing
            DetermineDirection(move);
        }

        if (spriteAnimator.runtimeAnimatorController == islandAnimator)
        {
            spriteAnimator.SetBool("change direction", false);

            if (direction != lastDirection)
            {
                lastDirection = direction;

                spriteAnimator.SetInteger("walking animation", direction);
                spriteAnimator.SetBool("change direction", true);
            }
        }
        if (spriteAnimator.runtimeAnimatorController == mapAnimator)
        {
            // Change the direction the map sprite looks by rotating it
            RotateMapSprite(direction);
        }
    }

    public void DetermineDirection(Vector2 move)
    {
        float x = move.x;
        float y = move.y;

        if (x == 0 && y == 0)
        {
            direction = lastDirection; // Maintain the last direction when idle
        }
        else if (Mathf.Abs(x) > Mathf.Abs(y))
        {
            direction = (x > 0) ? 1 : 3; // right or left
        }
        else if (Mathf.Abs(y) > Mathf.Abs(x))
        {
            direction = (y > 0) ? 0 : 2; // up or down
        }
        else
        {
            if (x > 0 && y > 0)
                direction = 4; // up-right
            else if (x < 0 && y > 0)
                direction = 5; // up-left
            else if (x > 0 && y < 0)
                direction = 6; // down-right
            else if (x < 0 && y < 0)
                direction = 7; // down-left
        }
    }

    public void CheckExitConditions()
    {
        if (moveAction.ReadValue<Vector2>() == Vector2.zero)
        {
            stateMachine.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
        if (spriteAnimator.runtimeAnimatorController == islandAnimator)
        {
            spriteAnimator.SetBool("exit", true);
        }
        if (spriteAnimator.runtimeAnimatorController == mapAnimator)
        {
        
        }
    }

    private void RotateMapSprite(int direction)
    {
        float playerRotationY = stateMachine.gameObject.transform.rotation.eulerAngles.y;

        switch (direction)
        {
            case 0: // up
                playerRotationY = 0f;
                break;
            case 1: // right
                playerRotationY = 90f;
                break;
            case 2: // down
                playerRotationY = 180f;
                break;
            case 3: // left
                playerRotationY = -90f;
                break;
            case 4: // up-right
                playerRotationY = 45f;
                break;
            case 5: // up-left
                playerRotationY = -45f;
                break;
            case 6: // down-right
                playerRotationY = 135f;
                break;
            case 7: // down-left
                playerRotationY = -135f;
                break;
        }

        stateMachine.gameObject.transform.rotation = Quaternion.Euler(0, playerRotationY, 0);
    }

    public StateType GetStateType()
    {
        return StateType.WALKING;
    }
}