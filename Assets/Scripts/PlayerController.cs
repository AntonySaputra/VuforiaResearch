using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPunCallbacks
{
    [HideInInspector]
    public int id;

    [Header("Info")]
    public string coalition;
    public bool selected = false;

    [Header("Component")]
    public Rigidbody rig;
    public Player photonPlayer;
    public TextMeshProUGUI playerNickname;
    private Transform canvas;
    private Transform arrow;
   



    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        coalition = player.NickName;


        GameManager.instance.players[id - 1] = this;

        if(!photonView.IsMine)
        {
            rig.isKinematic = true;
        }
        
    }

    private void Start()
    {
        playerNickname.text = photonPlayer.NickName;
        canvas = transform.Find("Canvas");
        arrow = canvas.Find("Arrow");
    }

    private void Update()
    {
        arrow.gameObject.SetActive(selected);
    }

    public void shoot()
    {
        GameManager.instance.photonView.RPC("DestroyTarget", RpcTarget.All, GameManager.instance.selectedTarget.transform.name);
    }

    public void initiateRespawnTarget()
    {
        GameManager.instance.photonView.RPC("RespawnAllTarget", RpcTarget.All);
    }

}
