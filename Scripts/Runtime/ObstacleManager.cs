using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class ObstacleManager : MonoBehaviour
    {
        public List<ObstacleObj> ObstacleList = new List<ObstacleObj>();
        public Model[] Models;
        public void AddObstacle(ElementAttbutes attbutes)
        {
            AddObstacle(attbutes.Model).SetObjAttbutes(attbutes);
        }
        public ObstacleObj AddObstacle(int model = 0)
        { 
            ObstacleObj obstacleController = Instantiate(Models[model].Prefab, transform).GetComponent<ObstacleObj>();
            obstacleController.model = model;
            ObstacleList.Add(obstacleController);
            return obstacleController;
        }
    }
}
