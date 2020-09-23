using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class PedestrianManager: MonoBehaviour
    {
        public List<PedestrianController> PedestrainList = new List<PedestrianController>();
        public Model[] Models;
        public void AddPedestrian(ElementAttbutes attbutes)
        {
            AddPedestrian(attbutes.Model).SetObjAttbutes(attbutes);
        }
        public PedestrianController AddPedestrian(int model = 0)
        {
            PedestrianController pedestrianController = Instantiate(Models[model].Prefab, transform).GetComponent<PedestrianController>();
            pedestrianController.model = model;
            PedestrainList.Add(pedestrianController);
            return pedestrianController;
        }
    }

}
