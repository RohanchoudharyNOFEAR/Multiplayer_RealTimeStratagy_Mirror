using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Events;
using System;

public class Unit : NetworkBehaviour 
{

    [SerializeField] private UnityEvent onSelected = null;
    [SerializeField] private UnityEvent onDeSelected = null;
    public static event Action<Unit> ServerOnUnitSpawned;
    public static event Action<Unit> ServerOnUnitDespawned;

    public static event Action<Unit> AuthorityOnUnitSpawned;
    public static event Action<Unit> AuthorityOnUnitDespawned;

    [SerializeField] private Targeter targeter = null;
    [SerializeField] private UnitsMovement unitMovement = null;
    [SerializeField] private Health health = null;


    public UnitsMovement GetUnitMovement()
    {
        return unitMovement;
    }
    public Targeter GetTargeter()
    {
        return targeter;
    }


    #region Server

    public override void OnStartServer()
    {
        ServerOnUnitSpawned?.Invoke(this);
        health.ServerOnDie += ServerHandleDie;
    }

    public override void OnStopServer()
    {
        health.ServerOnDie -= ServerHandleDie;
        ServerOnUnitDespawned?.Invoke(this);
    }

    [Server]
    private void ServerHandleDie()
    {
        NetworkServer.Destroy(gameObject);
    }


    #endregion


    #region Client

    public override void OnStartAuthority()
    {
       

        AuthorityOnUnitSpawned?.Invoke(this);
    }

    public override void OnStopClient()
    {
      

        AuthorityOnUnitDespawned?.Invoke(this);
    }




    [Client]
    public void Select()
    {
        if (!isOwned) { return; }
        onSelected?.Invoke();
    }

    [Client]
    public void Deselect()
    {
        if (!isOwned) { return; }
        onDeSelected?.Invoke();
    }
    #endregion
}
