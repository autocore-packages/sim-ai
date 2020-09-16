using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class CheckPointManager : MonoBehaviour
    {
        public List<CheckPointController> CheckPointList = new List<CheckPointController>();
        public GameObject CheckPointPrefabs;


        public void AddCheckPoint(ElementAttbutes attbutes ,int obstacleYype = 0)
        {
            GameObject npc = Instantiate(CheckPointPrefabs ,transform);
            npc.GetComponent<ElementObject>().objAttbutes = attbutes;
            CheckPointList.Add(npc.GetComponent<CheckPointController>());
        }
    }

}
