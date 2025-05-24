using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class FallingDeath : MonoBehaviour
{
    [SerializeField]
    private Player_Battle_Controller playerCon;

    [SerializeField]
    private GameObject targetPlayer;


    [SerializeField]
    private Vector3 deathPos;

    [SerializeField]
    private Vector3 returnPos;

    private bool onFallingDeath = false;

    private bool onFadeOut = false;

    [SerializeField]
    private bool onDeath = false;

   
    public Vector3 ReturnPos { get => returnPos; set => returnPos = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       playerCon = targetPlayer.GetComponent<Player_Battle_Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (targetPlayer.transform.position.y <= deathPos.y&& !onFallingDeath)
        { 
         onFallingDeath = true;
            onFadeOut = true;

            if (!onDeath)
            { 
            targetPlayer.GetComponent<CharacterController>().enabled = false;
            }
            
        }

        if (!onFallingDeath) return;

        if (onFadeOut && !onDeath)
        {
            StartCoroutine(FallFadeOut());
            onFadeOut = false;
        }
        else if (onFadeOut && onDeath)
        { 
           StartCoroutine (FallFadeOutDeath());
            onFadeOut = false;
        }
       
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        StartCoroutine(FadeOut());
    //    }
    //}

    IEnumerator FallFadeOut()//復帰アリ
    { 
 
    gameObject.GetComponent<PlayableDirector>().Play();
    yield return new WaitForSeconds(2);
        playerCon.AllCancel();
        playerCon.AnimCon.SetBool("Landing", true);
    playerCon.transform.position = returnPos;
        targetPlayer.GetComponent<CharacterController>().enabled = true;
        onFallingDeath = false ;
    }

    IEnumerator FallFadeOutDeath()//復帰ナシ、ゲームオーバー
    {
        gameObject.GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds(2f);
        playerCon.FallingDie();
       
    }
}
