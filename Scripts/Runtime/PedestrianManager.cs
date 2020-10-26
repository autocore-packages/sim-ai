using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class PedestrianManager: MonoBehaviour
    {
        public List<PedestrianObj> PedestrainList = new List<PedestrianObj>();
        public Model[] Models;
        public void AddPedestrian(ElementAttbutes attbutes)
        {
            AddPedestrian(attbutes.Model).SetObjAttbutes(attbutes);
        }
        public PedestrianObj AddPedestrian(int model = 0)
        {
            PedestrianObj pedestrianController = Instantiate(Models[model].Prefab, transform).GetComponent<PedestrianObj>();
            pedestrianController.model = model;
            PedestrainList.Add(pedestrianController);
            return pedestrianController;
        }
    }

}
