using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class CheckPointManager : MonoBehaviour
    {
        public List<CheckPointController> CheckPointList = new List<CheckPointController>();
        [SerializeField]
        public Model[] Models;
        public void AddCheckPoint(ElementAttbutes attbutes)
        {
            AddCheckPoint(attbutes.Model).SetObjAttbutes(attbutes);
        }
        public CheckPointController AddCheckPoint(int model)
        {
            CheckPointController checkPointController= Instantiate(Models[model].Prefab, transform).GetComponent<CheckPointController>();
            checkPointController.model = model;
            CheckPointList.Add(checkPointController);
            return checkPointController;
        }
    }

}
