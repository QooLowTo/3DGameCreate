using System.Collections;
using UnityEngine;
/// <summary>
/// 動く床の動きを制御するクラスです。
/// </summary>
public class GroundTransform : MonoBehaviour
{
    private float moveVelocity;

    [SerializeField]
    private float moveVeloX;
    [SerializeField]
    private float moveVeloY;
    [SerializeField]
    private float moveVeloZ;

    [SerializeField]
    private float subVelo = 1;

    private float accelerationTime = 1f;
    [SerializeField]
    private float returnAccel;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveGround = gameObject.GetComponent<MoveGround>();

        moveVeloX = startPos.transform.position.x;
        moveVeloY = startPos.transform.position.y;
        moveVeloZ = startPos.transform.position.z;

        if (onMoveX)
        {
            //moveVeloX =  startPos.transform.position.x;
            moveLimit = limitPos.transform.localPosition.x;
            returnMoveLimit = startPos.transform.position.x;
      
        }

        if (onMoveY)
        {
            //moveVeloY = startPos.transform.position.y;
            moveLimit = limitPos.transform.localPosition.y;
            returnMoveLimit = startPos.transform.position.y;
        }

        if (onMoveZ)
        {
            //moveVeloZ = startPos.transform.position.z;
            moveLimit = limitPos.transform.localPosition.z;
            returnMoveLimit = startPos.transform.position.z;
        }
    }

    // Update is called once per frame
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

            moveVector = new Vector3(moveVeloX, moveVeloY, moveVeloZ);

            transform.position = moveVector;
        }
        else if (!moveStart && moveReturn)
        {
            if (moveVelocity <= returnMoveLimit)
            {
                moveReturn = false;
                accelerationTime = 0f;
                subVelo = 1f;
                returnAccel = 0f;
                firstMoveStart = false;
            }

            Acceleration();

            ReturnMoveControl();

            moveVector = new Vector3(moveVeloX, moveVeloY, moveVeloZ);

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
            subVelo = (returnAccel * accelerationValue);
            accelerationTime = returnAccel/* + 0.5f*/;
        }
    }

    private void MoveControl()
    {
        if (onMoveX)
        {
        
            moveVeloX += subVelo * Time.deltaTime;
            moveVelocity = moveVeloX;
        }

        if (onMoveY)
        {   

            moveVeloY += subVelo * Time.deltaTime;
            moveVelocity = moveVeloY;
        }

        if (onMoveZ)
        { 

            moveVeloZ += subVelo * Time.deltaTime;
            moveVelocity = moveVeloZ;
        }
    }

    private void ReturnMoveControl()
    {
        if (onMoveX)
        {

            moveVeloX -= subVelo * Time.deltaTime;
            moveVelocity = moveVeloX;
        }

        if (onMoveY)
        {

            moveVeloY -= subVelo * Time.deltaTime;
            moveVelocity = moveVeloY;
        }

        if (onMoveZ)
        {

            moveVeloZ -= subVelo * Time.deltaTime;
            moveVelocity = moveVeloZ;
        }
    }

    IEnumerator ReturnCount()
    {
        yield return new WaitForSeconds(5);
        accelerationTime = 0f;
        subVelo = 1f;
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
