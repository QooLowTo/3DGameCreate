using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
/// <summary>
/// プレイヤーの落下死を制御するクラスです。
/// </summary>
public class FallingDeath : MonoBehaviour
{
    private Player_Battle_Controller playerBattleCon;

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

    void Start()
    {
       playerBattleCon = targetPlayer.GetComponent<Player_Battle_Controller>();
    }

    void Update()
    {
        //ブロックごとに説明を書いて
        if (targetPlayer.transform.position.y <= deathPos.y && !onFallingDeath)
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

    IEnumerator FallFadeOut()//復帰アリ
    { 
 
            
    gameObject.GetComponent<PlayableDirector>().Play();
    yield return new WaitForSeconds(2f);　//マジックナンバー！！変数化してください
        playerBattleCon.AllCancel();
        playerBattleCon.AnimCon.SetBool("Landing", true);
    playerBattleCon.transform.position = returnPos;
        targetPlayer.GetComponent<CharacterController>().enabled = true;
        onFallingDeath = false ;
    }

    IEnumerator FallFadeOutDeath()//復帰ナシ、ゲームオーバー
    {
        gameObject.GetComponent<PlayableDirector>().Play();
        yield return new WaitForSeconds(2f);　//マジックナンバー！！変数化してください
        playerBattleCon.FallingDie();
       
    }
}
