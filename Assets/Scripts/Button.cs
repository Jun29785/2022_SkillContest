using Define;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public ButtonType Type;

    public void OnClick()
    {
        switch (Type) 
        {
            case ButtonType.Register:
                FinishManager.Instance.Register();
                break;
        }
    }
}
