using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITrafficLight
{
    /// <summary>
    /// Set Light
    /// </summary>
    /// <param name="light">1green  2yellow  3red </param>
    void SetLight(int light);
    bool CanPass { get; }
    Transform StopLine { get; }
}
