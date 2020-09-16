using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class ObstacleManager : MonoBehaviour
    {
        public List<ObstacleController> ObstacleList = new List<ObstacleController>();
        public GameObject[] ObstaclePrefabs;


        public void AddObstacle(ElementAttbutes attbutes, int obstacleYype=0)
        {
            GameObject npc = Instantiate(ObstaclePrefabs[obstacleYype],transform);
            ObstacleList.Add(npc.GetComponent<ObstacleController>());
        }
    }

}
