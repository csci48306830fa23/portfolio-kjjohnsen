using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnKeypress : MonoBehaviour
{
    [SerializeField]
    private AudioSource barkSource;
    [SerializeField]
    private AudioClip[] clips;
    int currentClip = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            currentClip = (currentClip + 1) % clips.Length;
            barkSource.clip = clips[currentClip];
            barkSource.Play();
            AudioSource.PlayClipAtPoint(clips[currentClip], Vector3.zero);
        }
    }
}
