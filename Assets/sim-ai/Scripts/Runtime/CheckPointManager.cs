using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class CheckPointManager : MonoBehaviour
    {
        public List<CheckPointObj> CheckPointList = new List<CheckPointObj>();
        [SerializeField]
        public Model[] Models;
        public void AddCheckPoint(ElementAttbutes attbutes)
        {
            AddCheckPoint(attbutes.Model).SetObjAttbutes(attbutes);
        }
        public CheckPointObj AddCheckPoint(int model)
        {
            CheckPointObj checkPointController= Instantiate(Models[model].Prefab, transform).GetComponent<CheckPointObj>();
            checkPointController.model = model;
            CheckPointList.Add(checkPointController);
            return checkPointController;
        }
    }

}
