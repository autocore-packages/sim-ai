using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class ObstacleManager : MonoBehaviour
    {
        public List<ObstacleController> ObstacleList = new List<ObstacleController>();
        public Model[] Models;
        public void AddObstacle(ElementAttbutes attbutes)
        {
            AddObstacle(attbutes.Model).SetObjAttbutes(attbutes);
        }
        public ObstacleController AddObstacle(int model = 0)
        {
            ObstacleController obstacleController = Instantiate(Models[model].Prefab, transform).GetComponent<ObstacleController>();
            obstacleController.model = model;
            ObstacleList.Add(obstacleController);
            return obstacleController;
        }
    }

}
