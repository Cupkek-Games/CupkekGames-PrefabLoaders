using UnityEngine;

namespace CupkekGames.PrefabLoaders
{
  public class PrefabLoaderReportDestroy : MonoBehaviour
  {
    [System.NonSerialized] public object PrefabKey;
    [System.NonSerialized] public IPrefabLoaderBase PrefabLoader;
    private void OnDestroy()
    {
      // Null when the component was added but never wired up (or the
      // loader reference was cleared during teardown).
      PrefabLoader?.ReportDestroy(PrefabKey, gameObject);
    }
  }
}