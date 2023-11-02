//using Siccity.GLTFUtility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TestGLTF : MonoBehaviour
{
	public string path = "https://models.readyplayer.me/653a98a903fbd3bd39f48836.glb";
	// Start is called before the first frame update
	void Start()
    {
        StartCoroutine(downloadAvatar(path));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator downloadAvatar(string url)
    {
		using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
		{
			// Request and wait for the desired page.
			yield return webRequest.SendWebRequest();


			switch (webRequest.result)
			{
				case UnityWebRequest.Result.ConnectionError:
				case UnityWebRequest.Result.DataProcessingError:
					Debug.LogError("Error: " + webRequest.error);
					break;
				case UnityWebRequest.Result.ProtocolError:
					Debug.LogError("HTTP Error: " + webRequest.error);
					break;
				case UnityWebRequest.Result.Success:
					Debug.Log("nReceived: " + webRequest.downloadHandler.data.Length);

					//GameObject avatar = Importer.LoadFromBytes(webRequest.downloadHandler.data);

					break;
			}
		}
	}
}
