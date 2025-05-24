using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// このボタンを使用するにはTMProが必要
/// </summary>
public class ButtonController : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IMoveHandler
{
   
    MultipleButtonController parentController;

    //今選択されているボタン
    GameObject selectedButton;

    EventSystem eventSystem;

    private SoundManager soundManager;

    private GameObject findSoundManager;

    private ButtonSelector buttonSelector;

    private GameObject findButtonSelector;

    public EventSystem EventSystem { get => eventSystem; set => eventSystem = value; }


    public void Start()
    {
        findSoundManager = GameObject.FindWithTag("SoundManager");

        findButtonSelector = GameObject.FindWithTag("ButtonSelector");

        soundManager = findSoundManager.GetComponent<SoundManager>();

        parentController = transform.parent.gameObject.GetComponent<MultipleButtonController>();

        buttonSelector = findButtonSelector.GetComponent<ButtonSelector>();

        eventSystem = EventSystem.current;
        //GM = findGm.GetComponent<GameManager>();

        //eventSystem = EventSystem.current;

    }

    //private void Update()
    //{
    //    //OnUpdateSelect();
       
    //}

    /// <summary>
    /// このボタンにマウスポインターが入ったら呼ばれる
    /// 注意：単体で使う場合専用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (parentController != null) return;

        //親無しの場合ここに処理を書く

        GameObject target = eventData.pointerEnter.transform.parent?.gameObject;

        buttonSelector.GetSelectButtonName();

        if (target != null && target == this.gameObject)
        {
            Debug.Log("eventData GameObject " + target.name);

            soundManager.OneShot_UI_Sound(2);//選択音

           
        }
        else
        {
            Debug.Log("target is null");
        }

        //Debug.Log("カーサ―は " + target.name + "に入った");
    }

    public void OnUpdateSelect()
    {
        if (selectedButton != null)
        {
            eventSystem.SetSelectedGameObject(selectedButton);
  
        }
    }
    /// <summary>
    /// キーボード、コントローラーで選択する際に使用。
    /// </summary>
    /// <param name="axis"></param>
    public void OnMove(AxisEventData axis)
    {
        GameObject target = axis.selectedObject.transform.gameObject;

      

        if (target != null && target == this.gameObject)
        {
            soundManager.OneShot_UI_Sound(3);//選択限度音
        }
        else
        {
            soundManager.OneShot_UI_Sound(2);//選択音

            buttonSelector.GetSelectButtonName();
        }

        
    }

    /// <summary>
    /// このボタンからマウスポインターが出たら呼ばれる
    /// 注意：単体で使う場合専用
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (parentController != null) return;

        //親無しの場合ここに処理を書く

        GameObject target = eventData.pointerEnter.transform.parent?.gameObject;

        if (target != null && target == this.gameObject)
        {
            Debug.Log("eventData GameObject " + target.name);

            //ここで処理を書く
        }
        else
        {
            Debug.Log("target is null");
        }

        //Debug.Log("カーサ―は " + target.name + "から出た");

    }

    public void OnMoveExit(AxisEventData axis)
    {
        GameObject target = axis.selectedObject.transform.gameObject;

        if (target != null && target == this.gameObject)
        {
            Debug.Log("eventData GameObject " + target.name);

            //ここで処理を書く
        }
        else
        {
            Debug.Log("target is null");
        }

        Debug.Log("カーサ―は " + target.name + "から出た");
    }

}
