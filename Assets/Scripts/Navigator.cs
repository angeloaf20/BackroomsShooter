using System;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour
{
    private Vector3 _home;
    [SerializeField] private NavMeshAgent _navmeshAgent;
    private Transform _target;

    private void Awake()
    {
        _home = transform.position; 
        // _navmeshAgent = GetComponentInParent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_target)
        {
            transform.parent.LookAt(_target);
            _navmeshAgent.SetDestination(_target.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _target = other.transform;
        _navmeshAgent.stoppingDistance = 12.5f;
    }


    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        _target = null;
        _navmeshAgent.SetDestination(_home);
        _navmeshAgent.stoppingDistance = 0.1f;
    }
}
