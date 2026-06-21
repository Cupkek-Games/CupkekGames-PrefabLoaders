using System;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using CupkekGames.KeyValueDatabases;
using CupkekGames.EditorInspector;

namespace CupkekGames.PrefabLoaders
{
  public abstract class PrefabLoader<TKey> : KeyValueDatabaseMono<TKey, GameObject>, IPrefabLoader<TKey, GameObject>
  {
    private Dictionary<TKey, List<GameObject>> _instances = new();
    [SerializeField] private FolderReference _searchFolder;

    // Event for unloading UI
    public event EventHandler<TKey> OnInstanceDestroyed;

    /// <summary>Live instances for a key, or null if none exist.</summary>
    public List<GameObject> GetInstances(TKey key)
    {
      return _instances.TryGetValue(key, out List<GameObject> list) ? list : null;
    }

    /// <summary>
    /// Instantiate the prefab registered under <paramref name="key"/>.
    /// At most one live instance per key: while an instance exists,
    /// further calls return null. The slot frees when the instance is
    /// destroyed (tracked via <see cref="PrefabLoaderReportDestroy"/>).
    /// </summary>
    public virtual GameObject Instantiate(TKey key)
    {
      if (!ContainsKey(key))
      {
        Debug.LogWarning("Key not found: " + key);

        return null;
      }

      if (_instances.ContainsKey(key))
      {
        Debug.LogWarning("Instance already exists for key, skipping instantiate: " + key);

        return null;
      }

      GameObject instance = Instantiate(GetValue(key).gameObject);

      AddReportDestroy(key, instance);

      _instances[key] = new List<GameObject> { instance };

      return instance;
    }

    public void DestroyAllOf(TKey key)
    {
      if (_instances.ContainsKey(key))
      {
        List<GameObject> list = _instances[key];
        foreach (GameObject go in list)
        {
          Destroy(go);
        }
        _instances.Remove(key);

        OnInstanceDestroyed?.Invoke(this, key);
      }
    }

    public IEnumerator DestroyAllOfWithDelay(TKey key, float duration)
    {
      yield return new WaitForSeconds(duration);

      // Instances may have been destroyed (and the key removed via
      // ReportDestroy) during the delay.
      if (_instances.TryGetValue(key, out List<GameObject> list))
      {
        foreach (GameObject go in list)
        {
          Destroy(go);
        }
      }
    }

    public void AddReportDestroy(object key, GameObject instance)
    {
      if (!instance.TryGetComponent<PrefabLoaderReportDestroy>(out var report))
      {
        report = instance.AddComponent<PrefabLoaderReportDestroy>();
      }

      report.PrefabLoader = this;
      report.PrefabKey = key;
    }

    public void ReportDestroy(object keyObj, GameObject instance)
    {
      TKey key = (TKey)keyObj;
      if (_instances.ContainsKey(key))
      {
        List<GameObject> list = _instances[key];
        if (list.Remove(instance))
        {
          if (list.Count == 0)
          {
            _instances.Remove(key);
          }

          OnInstanceDestroyed?.Invoke(this, key);
        }
      }
    }
    public void DestroyAll()
    {
      foreach (var key in Keys)
      {
        DestroyAllOf(key);
      }
    }
  }
}