using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class NPCManager : MonoBehaviour
    {
        public List<NPCController> CarList = new List<NPCController>();
        public ElementsManager elementsManager;
        public GameObject[] NPCPrefabs;


        public void AddNPC(int npcYype=0)
        {
            GameObject npc = Instantiate(NPCPrefabs[npcYype]);
            ElementsManager.Instance.ElementList.Add(npc.GetComponent<ElementObject>());
        }
    }

}
