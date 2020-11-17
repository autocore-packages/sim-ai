﻿#region License
/*
* Copyright 2018 AutoCore
*
* Licensed under the Apache License, Version 2.0 (the "License");
* you may not use this file except in compliance with the License.
* You may obtain a copy of the License at
*
*     http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing, software
* distributed under the License is distributed on an "AS IS" BASIS,
* WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
* See the License for the specific language governing permissions and
* limitations under the License.
*/
#endregion

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
