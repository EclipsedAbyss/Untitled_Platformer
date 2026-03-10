using UnityEngine;

public class AdvancedMovement : MonoBehaviour
{

    private BaseMovement baseMovement;
    public KeyCode QBKey = KeyCode.LeftShift;
    private float readyToQB;
    private float QBCoolDown;
    private void Start()
    {
        baseMovement = GetComponent<BaseMovement>();
    }
    private void Update()
    {
        InputRegisterADV();
    }

    private void InputRegisterADV()
    {

        if (Input.GetKey(QBKey) && baseMovement.DashCount > 0)
        {
            QuickBoost();
            baseMovement.DashCount -= 1;
            Invoke(nameof(ResetQB), QBCoolDown);
        }
    }

    private void QuickBoost()
    {

    }

    private void ResetQB()
    {

    }


}
