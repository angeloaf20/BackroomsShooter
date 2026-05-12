using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets.Scripts.Gun
{
    public class SingleHand : MonoBehaviour, IFPSHands
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private TwoBoneIKConstraint _primaryHand;
        [SerializeField] private RigBuilder _builder;
        [SerializeField] private Transform _primaryGrip;

        public void MoveHands(GunData gunData)
        {
            Debug.Log("Moving");
            _animator.CrossFadeInFixedTime("PistolIdle", 0.1f);
            _primaryHand.data.target = _primaryGrip;
            _builder.Build();
        }
    }
}
