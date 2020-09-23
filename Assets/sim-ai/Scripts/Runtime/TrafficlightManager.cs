using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.simai
{
    public class TrafficlightManager : MonoBehaviour
    {
        public List<TrafficLightController> TrafficLightList = new List<TrafficLightController>();

        public void SetTrafficlight(ElementAttbutes attbutes,string name)
        {
            if (TrafficLightList.Count == 0)
            {
                TrafficLightList= FindObjectsOfType<TrafficLightController>().ToList();
            }
            foreach (TrafficLightController trafficLight in TrafficLightList)
            {
                if (trafficLight.name == name)
                {
                    trafficLight.objAttbutes = attbutes;

                }
            }
        }

    }

}
