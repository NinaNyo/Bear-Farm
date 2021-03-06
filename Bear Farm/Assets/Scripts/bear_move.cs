﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class bear_move : MonoBehaviour
{
    NavMeshAgent navMeshAgent;
    NavMeshPath path;
    public float timeforNewPath;
    public float timeforNewDesire;
    bool inCoRoutine; //movement 용
    bool inCoRoutine2; //intearction 용


    Vector3 target;
    bool validPath;


    //to set animation
    public GameObject animate;
    AnimatorController animating;

    //desire check //이미 요구가 떠있다면 또 뜨지 않게
    public bool desire;

    //밤인지 아닌지 check
    public bool day;
    public GameObject DayNightManager;
    daynightchange daynightchange;

    //desire용 object 종류
    GameObject cake;



    void Awake()
    {
        timeforNewPath = Random.Range(10, 30);
        timeforNewDesire = Random.Range(20, 50);
        animating = animate.GetComponent<AnimatorController>();
        daynightchange = DayNightManager.GetComponent<daynightchange>();

    }

        // Use this for initialization
        void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        string check_cake;
        check_cake = "Cake_" + gameObject.name.Substring(8);
        cake = GameObject.Find(check_cake);
        cake.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        if (daynightchange.check_day) //낮일 때 움직임
        {

            navMeshAgent.isStopped = false;
            //요구 안떠있을 때
            if (!desire)
            {
                if (!inCoRoutine)
                {
                    //움직임
                    StartCoroutine(Movement());

                }
                if (!inCoRoutine2)
                {
                    StartCoroutine(Desire());
                }
                if (navMeshAgent.velocity == Vector3.zero)
                    animating.SetInt("animation, 1");
                else
                    animating.SetInt("animation, 15");
            }
            else
            {
                //요구 떠있는 상태면
                animating.SetInt("animation,9");
                navMeshAgent.isStopped = true;
            }
        
        }
        else //밤일때 눕기
        {
            animating.SetInt("animation, 6");
            navMeshAgent.isStopped = true;
            cake.SetActive(false);
            desire = false;

        }


    }

    Vector3 getNewRandomPosition()
    {
        float x = Random.Range(-10, 10);
        float z = Random.Range(-10, 10);

        Vector3 pos = new Vector3(x, 0, z);
        return pos;
    }

    IEnumerator Movement()
    {

        inCoRoutine = true;
        timeforNewPath = Random.Range(10, 30);
        yield return new WaitForSeconds(timeforNewPath);

        GetNewPath();
        validPath = navMeshAgent.CalculatePath(target, path);
        if (!validPath)
            Debug.Log("Found an invalid path");

        while (!validPath)
        {
            yield return new WaitForSeconds(0.01f);
            GetNewPath();
            validPath = navMeshAgent.CalculatePath(target, path);
        }
        inCoRoutine = false;


    }

    void GetNewPath()
    {
        
        target = getNewRandomPosition();
        navMeshAgent.SetDestination(target);

    }


    ///Interaction
    IEnumerator Desire()
    {

        inCoRoutine2 = true;
        timeforNewDesire = Random.Range(20, 50);
        yield return new WaitForSeconds(timeforNewDesire);
        cake.SetActive(true);
        desire = true;


        inCoRoutine2 = false;


    }




}