using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VelUtils;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    Rig rig;
    [SerializeField]
    Transform spawnLocation;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1.0f);
        ResetGame();
    }

    public void ResetGame()
    {
		Movement m = rig.GetComponent<Movement>();
		m.TeleportTo(spawnLocation.position, spawnLocation.forward);
	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
