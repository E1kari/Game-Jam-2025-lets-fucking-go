using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WalkingState : BaseState
{
    S_Player_Data playerData;
    StateType stateType = StateType.WALKING;

    StateMachine stateMachine;
    Rigidbody rb;
    InputAction moveAction;

    Animator animator;
    int direction = 2;
    private int lastDirection = 2;
    public static Vector2 playerDirection;

    public void Enter(StateMachine pa_stateMachine)
    {
        Debug.Log("entered walking state");
        playerData = Resources.Load<S_Player_Data>("Scriptable Objects/S_Player_Data");

        stateMachine = pa_stateMachine;
        moveAction = InputSystem.actions.FindAction("Move");
        rb = stateMachine.gameObject.GetComponent<Rigidbody>();

        GameObject player = stateMachine.gameObject;
        GameObject child = player.transform.GetChild(0).gameObject;
        animator = child.GetComponent<Animator>();
    }

    public void UpdateState()
    {
        CheckExitConditions();

        Vector2 move = moveAction.ReadValue<Vector2>();
        rb.linearVelocity = new Vector3(move.x * playerData.movementSpeed, 0, move.y * playerData.movementSpeed);

        playerDirection = move; // Save the direction the player is facing

        animator.SetBool("change direction", false);
        DetermineDirection(move);

        if (direction != lastDirection)
        {
            lastDirection = direction;

            animator.SetInteger("walking animation", direction);
            animator.SetBool("change direction", true);
        }
    }

    public void DetermineDirection(Vector2 move)
    {
        float x = move.x;
        float y = move.y;

        if (x == 0 && y == 0)
        {
            direction = 2; // idle
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
        animator.SetBool("exit", true);
    }

    public StateType GetStateType()
    {
        return StateType.WALKING;
    }
}