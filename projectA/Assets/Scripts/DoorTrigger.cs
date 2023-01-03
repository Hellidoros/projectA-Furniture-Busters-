using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField]private Door _door;
    private int _agentsInRange = 0;
    private Vector3 _monsterPosition;
    private Collider _boxCollider;

    private void Start()
    {
        foreach(Collider collider in GetComponents<Collider>())
        {
            if (collider.isTrigger)
            {
                _boxCollider = collider;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<NavMeshAgent>(out NavMeshAgent agent))
        {
            _agentsInRange++;
            if (!_door.isOpen)
            {
                _monsterPosition = other.transform.position;
                _door.isMonster = true;
                _door.Open(_monsterPosition);
                _boxCollider.enabled = false;
                StartCoroutine(CloseDoor());
            }
        }
    }

    IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(3.5f);
        _door.Close(_monsterPosition);
        yield return new WaitForSeconds(1f);
        _door.isMonster = false;
        _boxCollider.enabled = true;
    }

}
