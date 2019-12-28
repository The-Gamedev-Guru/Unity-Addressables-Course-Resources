// Copyright 2019 The Gamedev Guru (http://thegamedev.guru)
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.using System.Collections;

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Assertions;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace TheGamedevGuru
{
public class PerformancePool<T> : IDisposable
  where T : MonoBehaviour, IPerformancePoolComponent
{
  private AssetReference _prefabReference;
  private int _totalElementCount;
  private Stack<T> _availableComponentsPool = null;
  private Stack<T> _allComponents = null;
  private Coroutine _loadingCoroutine;
  private AsyncOperationHandle<GameObject> _prefabAsyncOperationHandle;

  public void Setup(AssetReference prefabReference, int totalElementCount)
  {
    _prefabReference = prefabReference;
    _totalElementCount = totalElementCount;

    Assert.IsNotNull(prefabReference, "Prefab to instantiate must be non-null");
    SetupPool();
  }

  public void Dispose()
  {
    foreach (var component in _allComponents)
    {
      // In case Unity destroyed it already.
      if (component)
      {
        Object.Destroy(component.gameObject);
      }
    }

    _availableComponentsPool = null;
    _allComponents = null;
    Addressables.Release(_prefabAsyncOperationHandle);
  }

  public T Take()
  {
    Assert.IsTrue(_prefabAsyncOperationHandle.IsValid());
    if (_availableComponentsPool.Count > 0)
    {
      var newComponent = _availableComponentsPool.Pop();
      newComponent.Activate();
      return newComponent;
    }

    return null;
  }

  public void Return(T componentObjectToReturn)
  {
    Assert.IsTrue(_prefabAsyncOperationHandle.IsValid());
    componentObjectToReturn.Deactivate();
    _availableComponentsPool.Push(componentObjectToReturn);
  }

  private void SetupPool()
  {
    _availableComponentsPool = new Stack<T>(_totalElementCount);
    _allComponents = new Stack<T>(_totalElementCount);
    _prefabAsyncOperationHandle = _prefabReference.LoadAssetAsync<GameObject>();
    _prefabAsyncOperationHandle.Completed += handle =>
    {
      var prefab = _prefabAsyncOperationHandle.Result;
      for (var i = 0; i < _totalElementCount; i++)
      {
        var newGameObject = Object.Instantiate(prefab);
        var component = newGameObject.GetComponent<T>();
        component.Deactivate();
        _availableComponentsPool.Push(component);
        _allComponents.Push(component);
      }
    };
  }
}
}