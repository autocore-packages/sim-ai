using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class NPCManager : MonoBehaviour
    {
        public List<NPCController> NPCList = new List<NPCController>();
        public GameObject[] NPCPrefabs;


        public void AddNPC(ElementAttbutes attbutes, int npcYype=0)
        {
            GameObject npc = Instantiate(NPCPrefabs[npcYype],transform);
            
            NPCList.Add(npc.GetComponent<NPCController>());
        }
    }

}
