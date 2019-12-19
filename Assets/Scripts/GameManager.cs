using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject imageTarget;

    [Header("Stats")]
    public bool gameEnded = false;
    public TextMeshProUGUI pingUI;

    [Header("Players")]
    public string playerPrefabLocation;
    public Transform[] spawnPoints;
    public PlayerController[] players;
    private int playersInGame;
    private List<int> pickedSpawnIndex;

    [Header("Targets")]
    public GameObject selectedTarget;


    //instance
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        pickedSpawnIndex = new List<int>();
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.AllBuffered);
    }

    private void Update()
    {
        pingUI.text = "Cloud Region: " + PhotonNetwork.CloudRegion +
                   "\n Ping: " + PhotonNetwork.GetPing().ToString() + "ms";
    }

    [PunRPC]
    void ImInGame()
    {
        playersInGame++;

        if (playersInGame == PhotonNetwork.PlayerList.Length)
            SpawnPlayer();
    }

    void SpawnPlayer()
    {

        int[] index = new int[2];
        if (PhotonNetwork.IsMasterClient)
        {
            index[0] = 0;
            index[1] = 2;
        }
        else
        {
            index[0] = 1;
            index[1] = 3;
        }
            

        GameObject playerObject = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[index[0]].position, Quaternion.identity);
        playerObject.transform.name = "Player " + index[0];

        
        //initialize the player
        PlayerController playerScript = playerObject.GetComponent<PlayerController>();
        playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        playerObject.transform.SetParent(imageTarget.transform);
        //playerObject.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

        GameObject playerObject2 = PhotonNetwork.Instantiate(playerPrefabLocation, spawnPoints[index[1]].position, Quaternion.identity);
        playerObject2.transform.name = "Player " + index[1];


        //initialize the player
        PlayerController playerScript2 = playerObject2.GetComponent<PlayerController>();
        playerScript2.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        playerObject2.transform.SetParent(imageTarget.transform);
        //playerObject2.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);


    }

    [PunRPC]
    void DestroyTarget(string targetName)
    {
        GameObject target = GameObject.Find(targetName);
        if(target)
        {
            target.SetActive(false);
        }
        else
        {
            print("target not found");
        }
    }

    [PunRPC]
    void RespawnAllTarget()
    {
        if(!CheckTargetExist())
        {
            GameObject targets = GameObject.Find("_TargetObject");
            foreach(Transform target in targets.transform)
            {
                target.gameObject.SetActive(true);
            }
        }
    }

    bool CheckTargetExist()
    {
        GameObject targets = GameObject.Find("_TargetObject");
        foreach(Transform target in targets.transform)
        {
            if(target.gameObject.activeInHierarchy)
            {
                return true;
            }
        }

        return false;
    }

    public PlayerController GetPlayer (int playerID)
    {
        return players.First(x => x.id == playerID);
    }

    public PlayerController GetPlayer(GameObject playerObj)
    {
        return players.First(x => x.gameObject == playerObj);
    }
}
