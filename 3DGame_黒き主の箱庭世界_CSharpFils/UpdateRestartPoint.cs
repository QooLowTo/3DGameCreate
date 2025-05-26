using UnityEngine;

/// <summary>
/// プレイヤーが落下死した際に復活地点を設定するクラスです。
/// </summary>
public class UpdateRestartPoint : MonoBehaviour
{
    [SerializeField]
    private FallingDeath fallingDeath;
    [SerializeField]
    private GameObject findfallDeath;

    [SerializeField]
    private GameObject returnPoint;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fallingDeath = findfallDeath.GetComponent<FallingDeath>();
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
          fallingDeath.ReturnPos = returnPoint.transform.position;
            Destroy(gameObject);
        }
    }
}
