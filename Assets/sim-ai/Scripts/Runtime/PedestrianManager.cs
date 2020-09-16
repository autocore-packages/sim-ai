using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class PedestrianManager: MonoBehaviour
    {
        public List<PedestrianController> PedestrainList = new List<PedestrianController>();
        public GameObject[] NPCPrefabs;


        public void AddPedestrian(ElementAttbutes attbutes, int PedYype =0)
        {
            GameObject Ped = Instantiate(NPCPrefabs[PedYype ],transform);
            PedestrainList.Add(Ped.GetComponent<PedestrianController>());
        }
    }

}
