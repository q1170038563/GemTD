﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower_L : Tower
{
    private Tower_C towerC;
    private Tower_I towerI;

    public int ThroughChance = 2;

    void Start()
    {
        currentAttackValue = AttackValue;
        currentInterval = AttackInterval;
        Enemies = new Queue<GameObject>();
    }

    public override void AttackBehavior()
    {
        if (Traget != null)
        {
            Traget.GetComponent<Enemy>().Hurt(AttackValue);
            shebao(Traget);
            if (Random.Range(0, 10) % 10 < ThroughChance)
            {
                wtmsb();
            }
        }
        else
        {
            if (Enemies.Count > 0)
            {
                Traget = Enemies.Dequeue();
            }
        }
    }

    void wtmsb()
    {
        RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, 100f, 1 << LayerMask.NameToLayer("Enemy"));

        // todo she bao


        for (int i = hits.Length; i >= 0; i--)
        {
            hits[i].transform.GetComponent<Enemy>().Hurt((int)(currentAttackValue * 1.5));
        }
    }

    void Update()
    {
        Attack();
        LookAtTraget();
        if (Traget)
            Debug.DrawLine(transform.position, Traget.transform.position, Color.red);

        CheckTowerC();
        Accelerate();

        CheckTowerI();
        IncreasedAttack();
    }

    void IncreasedAttack()
    {
        if (towerI)
            currentAttackValue = (int)(AttackValue * (1 + towerI.AttackInterval));
        else
            currentAttackValue = AttackValue;
    }

    void CheckTowerI()
    {
        if (!towerI)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f, 1 << LayerMask.NameToLayer("I"));
            if (colliders.Length > 0)
            {
                Collider collider = colliders[0];
                float distance = collider.gameObject.GetComponent<Tower_I>().AttackRange;
                if (Vector3.Distance(transform.position, collider.transform.position) <= distance)
                    towerI = collider.gameObject.GetComponent<Tower_I>();
            }
        }
    }

    void Accelerate()
    {
        if (towerC)
        {
            currentInterval = AttackInterval * (1 - towerC.AcceleratePercentage);
        }
        else
        {
            currentInterval = AttackInterval;
        }
    }

    void CheckTowerC()
    {
        if (!towerC)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f, 1 << LayerMask.NameToLayer("C"));
            if (colliders.Length > 0)
            {
                Collider collider = colliders[0];
                float distance = collider.gameObject.GetComponent<Tower_C>().AttackRange;
                if (Vector3.Distance(transform.position, collider.transform.position) <= distance)
                    towerC = collider.gameObject.GetComponent<Tower_C>();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Enemy")
        {
            if (Traget == null && Enemies.Count <= 0)
            {
                Debug.Log("Set Traget");
                Traget = other.gameObject;
            }
            else if (Traget != null)
            {
                Debug.Log("Add Enemies");
                Enemies.Enqueue(other.gameObject);
            }
        }

        //Debug.Log(Traget);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.Equals(Traget))
        {
            Traget = null;
        }
        else
        {
            if (Enemies.Count > 0)
                Enemies.Dequeue();
        }
        //Debug.Log(Traget);
    }
}
