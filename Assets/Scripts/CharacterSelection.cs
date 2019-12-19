using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CharacterSelection : MonoBehaviour
{
    //public Image selectedIndicator;
    private List<GameObject> units;

    private void Awake()
    {
      
    }
    void Update()
    {
        units = GetAllUnits(PhotonNetwork.NickName);
        if (Input.GetMouseButtonDown(0))
        {
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if(hit.transform.tag == "Player")
                {
                    
                    if(units.Contains(hit.transform.gameObject))
                    {
                        
                        PlayerController playerScript = hit.transform.gameObject.GetComponent<PlayerController>();
                        Transform canvas = hit.transform.Find("Canvas");
                        Transform arrow = canvas.transform.Find("Arrow");

                        if (playerScript.selected)
                            playerScript.selected = false;
                        else
                        {
                            UnitClearSelection();
                            playerScript.selected = true;
                        }

                        
                    }
                }
            }
        }
    }

    List<GameObject> GetAllUnits(string coalition)
    {
        List<GameObject> unitsObject = new List<GameObject>();

        for (int i = 0; i < 4; i++)
        {
            if(GameObject.Find("Player " + i))
            {
                GameObject unit = GameObject.Find("Player " + i);
                PlayerController player = unit.gameObject.GetComponent<PlayerController>();
                if (player.coalition == coalition)
                    unitsObject.Add(unit);
            }
           
        }

        return unitsObject;
    }

    void UnitClearSelection()
    {
        foreach(GameObject unit in units)
        {
            PlayerController player = unit.gameObject.GetComponent<PlayerController>();
            player.selected = false;
        }
    }
}
