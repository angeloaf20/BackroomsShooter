using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Assets.Scripts.Gun
{
    public class BothHands : MonoBehaviour, IFPSHands
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private Transform _primaryTarget;
        [SerializeField] private Transform _secondaryTarget;
        [SerializeField] private TwoBoneIKConstraint _primaryHand;
        [SerializeField] private TwoBoneIKConstraint _secondaryHand;
        [SerializeField] private RigBuilder _builder;

        public void MoveHands(GunData gunData)
        {
            _animator.CrossFadeInFixedTime("RifleIdle", 0.1f);
            _primaryHand.data.target = _primaryTarget;
            _secondaryHand.data.target = _secondaryTarget;
            _builder.Build();
        }
    }
}
