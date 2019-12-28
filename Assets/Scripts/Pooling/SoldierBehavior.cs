using UnityEngine;

namespace TheGamedevGuru
{
public class SoldierBehavior : MonoBehaviour, IPerformancePoolComponent
{
  [SerializeField] private MeshRenderer _meshRenderer = null;

  public void Activate()
  {
    // More examples:
    // _aiComponent.enabled = true;
    // _animator.enabled = true;
    _meshRenderer.enabled = true;
    Debug.Log("Soldier ready");
  }

  public void Deactivate()
  {
    // More examples:
    // _aiComponent.enabled = false;
    // _animator.enabled = false;
    Debug.Log("Soldier offline");
    _meshRenderer.enabled = false;
    transform.SetParent(null, true); // For demo purposes.
  }
}
}