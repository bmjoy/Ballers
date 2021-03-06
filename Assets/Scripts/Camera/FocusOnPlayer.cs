﻿using MLAPI;
using MLAPI.Spawning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusOnPlayer : MonoBehaviour
{
    public bool startDisabled = true;
    public bool GetLocalPlayerObject = true;
    public Vector3 offset;
    public float rotation;

    private GameObject m_target;
    private bool m_loaded;

    void Start()
    {
        if (GetLocalPlayerObject)
            GameManager.Singleton.LocalPlayerLoaded += OnLocalPlayerLoaded;

        if (startDisabled)
            gameObject.SetActive(false);

        transform.Rotate(rotation, 0, 0);
    }

    void Update()
    {
        if (!m_loaded) return;
        if (m_target == null) SetTarget();

        transform.position = m_target.transform.position + offset;
    }

    private void OnLocalPlayerLoaded(Player p)
    {
        m_loaded = true;
        SetTarget();
    }

    private void SetTarget()
    {
        if (GetLocalPlayerObject)
        {
            m_target = SpawnManager.GetLocalPlayerObject()?.gameObject;
        }
        else
        {
            m_target = GameObject.Find("Player");
        }
    }
}
