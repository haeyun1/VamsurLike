using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // 프리펩들을 보관할 변수
    public GameObject[] prefabs;
    // 풀 담당을 하는 리스트들
    List<GameObject>[] pools;

    void Awake()
    {
        pools = new List<GameObject>[prefabs.Length];
        for (int i = 0; i < pools.Length; i++)
        {
            pools[i] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // 선택한 풀의 놀고 있는 (비활성화) 게임 오브젝트 접근
        
        foreach (GameObject item in pools[index])
        {
            // 발견하면 select 변수에 할당
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }
        // 못 찾았으면?
        if(!select)
        {
            // 새롭게 생성하고 select 변수에 할당
            select = Instantiate(prefabs[index], transform); // poolmanager의 자식 오브젝트에 생성하겠다
            pools[index].Add(select);
        }
         
        return select;
    }
}
