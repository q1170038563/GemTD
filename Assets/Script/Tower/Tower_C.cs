﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tower_C : Tower
{
    private Tower_I towerI;
    public float AcceleratePercentage = 0.2f;

    void Start()
    {
        currentInterval = AttackInterval;
        currentInterval = AttackInterval;
        Enemies = new Queue<GameObject>();
    }

    public override void AttackBehavior()
    {
        if (Traget)
        {
            Traget.GetComponent<Enemy>().Hurt(AttackValue);
        }
        else
        {
            if (Enemies.Count > 0)
            {
                Traget = Enemies.Dequeue();
            }
        }
    }

    void Update()
    {
        Attack();
        LookAtTraget();
        if (Traget)
            Debug.DrawLine(transform.position, Traget.transform.position, Color.red);

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

        //if (other.GetComponent<Tower>() != null && other.gameObject.name != gameObject.name && other.isTrigger)
        //{
        //    if (!other.GetComponent<Tower>().IsAccelerated)
        //    {
        //        Debug.Log(other.name);
        //        other.GetComponent<Tower>().IsAccelerated = true;
        //        other.GetComponent<Tower>().currentInterval *= (1 - AcceleratePercentage);
        //    }            
        //}

        //Debug.Log(Traget);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy")
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
        }

        if (other.GetComponent<Tower>() != null)
        {
            other.GetComponent<Tower>().currentInterval = AttackInterval;
            other.GetComponent<Tower>().IsAccelerated = false;
        }
    }
}
