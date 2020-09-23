using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class NPCManager : MonoBehaviour
    {
        public List<NPCController> NPCList = new List<NPCController>();
        public Model[] Models;
        public void AddNPC(ElementAttbutes attbutes)
        {
            AddNPC(attbutes.Model).SetObjAttbutes(attbutes);
        }
        public NPCController AddNPC(int model = 0)
        {
            NPCController npcController = Instantiate(Models[model].Prefab, transform).GetComponent<NPCController>();
            npcController.model = model;
            NPCList.Add(npcController);
            return npcController;
        }
    }

}
