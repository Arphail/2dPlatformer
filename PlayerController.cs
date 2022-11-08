using UnityEngine;

[CreateAssetMenu(fileName = "PlayerController", menuName = "InputController/PlayerController")]

public class PlayerController : InputController
{
    public override float RetrieveMoveInput() => Input.GetAxisRaw("Horizontal");

    public override bool RetrieveJumpInput() => Input.GetKeyDown(KeyCode.Space);
}
