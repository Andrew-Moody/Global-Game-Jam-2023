using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTest : MonoBehaviour
{
    public int ItemID;

    public int MobID;

    public Transform MobPos;


    // Start is called before the first frame update
    void Start()
    {
        ItemManager.Instance.SpawnWorldItem(ItemID, transform.position);

        MobManager.Instance.SpawnEnemy(MobID, MobPos.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
