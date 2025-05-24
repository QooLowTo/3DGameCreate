using Unity.Services.Analytics.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ���̃{�^�����g�p����ɂ�TMPro���K�v
/// </summary>
public class ButtonController : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler, IMoveHandler
{
   
    MultipleButtonController parentController;

    //���I������Ă���{�^��
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
    /// ���̃{�^���Ƀ}�E�X�|�C���^�[����������Ă΂��
    /// ���ӁF�P�̂Ŏg���ꍇ��p
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (parentController != null) return;

        //�e�����̏ꍇ�����ɏ���������

        GameObject target = eventData.pointerEnter.transform.parent?.gameObject;

        buttonSelector.GetSelectButtonName();

        if (target != null && target == this.gameObject)
        {
            Debug.Log("eventData GameObject " + target.name);

            soundManager.OneShot_UI_Sound(2);//�I����

           
        }
        else
        {
            Debug.Log("target is null");
        }

        //Debug.Log("�J�[�T�\�� " + target.name + "�ɓ�����");
    }

    public void OnUpdateSelect()
    {
        if (selectedButton != null)
        {
            eventSystem.SetSelectedGameObject(selectedButton);
  
        }
    }
    /// <summary>
    /// �L�[�{�[�h�A�R���g���[���[�őI������ۂɎg�p�B
    /// </summary>
    /// <param name="axis"></param>
    public void OnMove(AxisEventData axis)
    {
        GameObject target = axis.selectedObject.transform.gameObject;

      

        if (target != null && target == this.gameObject)
        {
            soundManager.OneShot_UI_Sound(3);//�I�����x��
        }
        else
        {
            soundManager.OneShot_UI_Sound(2);//�I����

            buttonSelector.GetSelectButtonName();
        }

        
    }

    /// <summary>
    /// ���̃{�^������}�E�X�|�C���^�[���o����Ă΂��
    /// ���ӁF�P�̂Ŏg���ꍇ��p
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        if (parentController != null) return;

        //�e�����̏ꍇ�����ɏ���������

        GameObject target = eventData.pointerEnter.transform.parent?.gameObject;

        if (target != null && target == this.gameObject)
        {
            Debug.Log("eventData GameObject " + target.name);

            //�����ŏ���������
        }
        else
        {
            Debug.Log("target is null");
        }

        //Debug.Log("�J�[�T�\�� " + target.name + "����o��");

    }

    public void OnMoveExit(AxisEventData axis)
    {
        GameObject target = axis.selectedObject.transform.gameObject;

        if (target != null && target == this.gameObject)
        {
            Debug.Log("eventData GameObject " + target.name);

            //�����ŏ���������
        }
        else
        {
            Debug.Log("target is null");
        }

        Debug.Log("�J�[�T�\�� " + target.name + "����o��");
    }

}
