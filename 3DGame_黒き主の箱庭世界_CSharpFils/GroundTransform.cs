using System.Collections;
using UnityEngine;
/// <summary>
/// 動く床の動きを制御するクラスです。
/// </summary>
public class GroundTransform : MonoBehaviour
{
    private float moveVelocity;

    [SerializeField]
    private float moveVelocityX;
    [SerializeField]
    private float moveVelocityY;
    [SerializeField]
    private float moveVelocityZ;

    [SerializeField]
    private float subVelocity = 1; //説明書いて

    private float accelerationTime = 1f;
    [SerializeField]
    private float returnAccel; //この名前どうかな速度？

    [SerializeField]
    private float accelerationValue = 20f;

    private Vector3 moveVector;

    [SerializeField]
    private GameObject startPos;

    [SerializeField]
    private GameObject limitPos;

    [SerializeField]
    bool firstMoveStart = false;
    [SerializeField]
    bool moveStart = false;
    [SerializeField]
    bool moveReturn = false;

    [SerializeField]
    private float moveLimit;
    [SerializeField]
    private float returnMoveLimit;

    [SerializeField]
    bool onMoveX = true;
    [SerializeField]
    bool onMoveY = false;
    [SerializeField]
    bool onMoveZ = false;

    private MoveGround moveGround;

    void Start()
    {
        moveGround = gameObject.GetComponent<MoveGround>();

        moveVelocityX = startPos.transform.position.x;
        moveVelocityY = startPos.transform.position.y;
        moveVelocityZ = startPos.transform.position.z;

        if (onMoveX)
        {
            moveLimit = limitPos.transform.localPosition.x;
            returnMoveLimit = startPos.transform.position.x;
      
        }

        if (onMoveY)
        {
            moveLimit = limitPos.transform.localPosition.y;
            returnMoveLimit = startPos.transform.position.y;
        }

        if (onMoveZ)
        {
            moveLimit = limitPos.transform.localPosition.z;
            returnMoveLimit = startPos.transform.position.z;
        }
    }

    void FixedUpdate()
    {
        if (moveStart)
        {
            if (!onMoveX && !onMoveY && !onMoveZ)
            {
                return;
            }


            if (moveLimit <= moveVelocity)
            {
                moveStart = false;
                StartCoroutine(ReturnCount());

            }

            Acceleration();

            MoveControl();

            moveVector = new Vector3(moveVelocityX, moveVelocityY, moveVelocityZ);

            transform.position = moveVector;
        }
        else if (!moveStart && moveReturn)
        {
            if (moveVelocity <= returnMoveLimit)
            {
                moveReturn = false;
                accelerationTime = 0f;
                subVelocity = 1f;
                returnAccel = 0f;
                firstMoveStart = false;
            }

            Acceleration();

            ReturnMoveControl();

            moveVector = new Vector3(moveVelocityX, moveVelocityY, moveVelocityZ);

            transform.position = moveVector;
        }
        else
        {
            return;
        }

    }

    private void Acceleration()
    {
        accelerationTime -= Time.deltaTime;

        if (accelerationTime <= 0)
        {

            returnAccel += 1f;
            subVelocity = (returnAccel * accelerationValue);
            accelerationTime = returnAccel/* + 0.5f*/;
        }
    }

    private void MoveControl()
    {
        if (onMoveX)
        {
        
            moveVelocityX += subVelocity * Time.deltaTime;
            moveVelocity = moveVelocityX;
        }

        if (onMoveY)
        {   

            moveVelocityY += subVelocity * Time.deltaTime;
            moveVelocity = moveVelocityY;
        }

        if (onMoveZ)
        { 

            moveVelocityZ += subVelocity * Time.deltaTime;
            moveVelocity = moveVelocityZ;
        }
    }

    private void ReturnMoveControl()
    {
        if (onMoveX)
        {

            moveVelocityX -= subVelocity * Time.deltaTime;
            moveVelocity = moveVelocityX;
        }

        if (onMoveY)
        {

            moveVelocityY -= subVelocity * Time.deltaTime;
            moveVelocity = moveVelocityY;
        }

        if (onMoveZ)
        {

            moveVelocityZ -= subVelocity * Time.deltaTime;
            moveVelocity = moveVelocityZ;
        }
    }

    IEnumerator ReturnCount()
    {
        yield return new WaitForSeconds(5);
        accelerationTime = 0f;
        subVelocity = 1f;
        returnAccel = 0f;

        moveReturn = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (firstMoveStart) return;

        if (other.gameObject.CompareTag("Player"))
        {
          firstMoveStart = true;
            moveStart = true;
        }
    }

}
