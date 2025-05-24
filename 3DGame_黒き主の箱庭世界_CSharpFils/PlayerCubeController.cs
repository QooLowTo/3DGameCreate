using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerCubeController : MonoBehaviour
{
   
    private GameObject findPlayer;

    private GameObject findGM;

    private PlayableDirector playable;

    private SoundManager soundManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        findPlayer = GameObject.FindWithTag("MainCamera");

        findGM = GameObject.FindWithTag("SoundManager");

        transform.LookAt(findPlayer.transform.position);

        playable = GetComponent<PlayableDirector>();

        soundManager = findGM.GetComponent<SoundManager>();


        soundManager.OneShot_Player_Sound(13);//スキル発動サウンド

        playable.Play();

    }

    public void DestroyCube()
    { 
      Destroy(gameObject);
    }

    // Update is called once per frame
    //void Update()
    //{
        
    //}
}
