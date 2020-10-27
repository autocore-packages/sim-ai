using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class NPCManager : MonoBehaviour
    {
        public List<NPCObj> NPCList = new List<NPCObj>();
        public Model[] Models;
        public void AddNPC(ElementAttbutes attbutes)
        {
            AddNPC(attbutes.Model).SetObjAttbutes(attbutes);
        }
        public NPCObj AddNPC(int model = 0)
        {
            NPCObj npc = Instantiate(Models[model].Prefab, transform).GetComponent<NPCObj>();
            npc.model = model;
            NPCList.Add(npc);
            return npc;
        }
    }

}
