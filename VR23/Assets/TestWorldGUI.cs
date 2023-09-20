using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWorldGUI : MonoBehaviour
{

    [SerializeField]
    Transform boxToHide;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void buttonPressed()
    {
        if (boxToHide != null)
        {
            boxToHide.GetComponent<Renderer>().enabled = false;
        }
    }
}
