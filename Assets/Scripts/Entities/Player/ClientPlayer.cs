﻿using MLAPI;
using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientPlayer : NetworkedBehaviour
{
    public static ClientPlayer Singleton { get; private set; }

    public int status = 0;

    public int Cid { get { return (UserData != null) ? UserData.lastChar : 0; } }
    public UserData UserData { get; private set; }
    public CharacterData CharData
    { 
        get {
            characterStats.TryGetValue(Cid, out CharacterData cData);
            return cData;
        }
        private set
        {
            characterStats.Add(Cid, value);
        }
    }
    public ulong SteamId { get; private set; }

    // This is cached character data. Can be used server or client side.
    // Primarily for non essential or non gameplay tasks. ie. character selection menu
    public Dictionary<int, CharacterData> characterStats = new Dictionary<int, CharacterData>();
    public float lastCharacterUpdate;

    private GameSetup m_gameSetup;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Singleton = this;
    }

    void Start()
    {
        if (SteamManager.Initialized)
            SteamId = SteamUser.GetSteamID().m_SteamID;

        m_gameSetup = GameObject.Find("GameManager").GetComponent<GameSetup>();
        StartCoroutine(Load());
    }

    void OnDestroy()
    {
        BackendManager.SaveCharacter(SteamId, Cid, CharData);
    }

    // Loads locally
    public IEnumerator Load()
    {
        yield return null;

        yield return BackendManager.Login(SteamId, LoginCallback);

        yield return BackendManager.FetchCharacterFromServer(SteamId, Cid, FetchCharacterCallback);

        yield return BackendManager.FetchAllCharacters(SteamId, FetchAllCharacterCallback);

        yield return null;

        m_gameSetup.hasClientLoaded = true;
        status = 1;
        print("Finished loading client");
    }

    public IEnumerator ReLoadCharacters()
    {
        yield return BackendManager.FetchAllCharacters(SteamId, FetchAllCharacterCallback);
    }

    // Logins to server
    private void LoginCallback(UserData uData, string err)
    {
        UserData = uData;
    }

    private void FetchCharacterCallback(CharacterData cData, string err)
    {
        CharData = cData;
        UserData.lastChar = cData.cid;
    }

    private void FetchAllCharacterCallback(List<CharacterData> cData, string err)
    {
        foreach (CharacterData c in cData)
        {
            characterStats[c.cid] = c;
        }
        lastCharacterUpdate = Time.time;
    }

    public void ChangeCharacter(int cid)
    {
        StartCoroutine(ChangeCharacterCoroutine(cid));
    }

    private IEnumerator ChangeCharacterCoroutine(int cid)
    {
        yield return BackendManager.SaveCharacter(SteamId, Cid, CharData);
        yield return null;
        yield return BackendManager.FetchCharacterFromServer(SteamId, cid, FetchCharacterCallback);
    }

}
