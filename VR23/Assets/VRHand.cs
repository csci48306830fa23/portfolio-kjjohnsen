using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VRHand : MonoBehaviour
{
    [SerializeField]
    InputAction fireAction;
    // Start is called before the first frame update
    void Start()
    {
        fireAction.Enable();
        fireAction.performed += (obj) => {
            Debug.Log("fire performed");
        };
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(fireAction.ReadValue < float>());
    }
}
