﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {
    public GameObject Enemy;

    public Transform CreatePoint;

    public float CreateInterval = 1;

    public int EnemyNum = 10;

    private float CreateTime;

    static List<GameObject> enemies;

    static bool isAttack = false;

    void Start()
    {
        CreateTime = Time.time;
        enemies = new List<GameObject>();
    }

    void Update()
    {
        if (enemies.Count <= 0 && isAttack)
        {
            isAttack = false;
            GameStateManager.ChangeState(BuildState.Instance);
        }
    }

    public void StartAttack()
    {
        StartCoroutine(Attack());        
    }

    public IEnumerator Attack()
    {
        GameObject enemy;
        for (int i = 0; i < EnemyNum; i++)
        {
            //Debug.Log(Time.time - CreateTime > CreateInterval);
            yield return new WaitUntil(() => (Time.time - CreateTime > CreateInterval));
            //if (Time.time - CreateTime > CreateInterval)
            CreateTime = Time.time;
            enemy = Instantiate(Enemy, CreatePoint.position, Quaternion.Euler(new Vector3(0, 180, 0))) as GameObject;
            enemy.GetComponent<NavPlayer>().isMove = true;
            enemies.Add(enemy);    
        }
        isAttack = true;
    }

    public static void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
    }

    public static int GetEnemiesCount()
    {
        return enemies.Count;
    }
}
