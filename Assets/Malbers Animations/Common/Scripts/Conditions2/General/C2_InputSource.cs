﻿using MalbersAnimations.Scriptables;
using UnityEngine;

namespace MalbersAnimations.Conditions
{
    [System.Serializable, AddTypeMenu("General/Input Source")]
    public class C2_InputSource : ConditionCore
    {
        public override string DynamicName => $"Input Is Pressed: [{inputName.Value}]";

        private IInputSource inputSource;
        private IInputAction input;

        public StringReference inputName = new("input");

        protected override void _SetTarget(Object target)
        {
            inputSource ??= MTools.VerifyInterface(target, inputSource);
            if (inputSource != null)
                input = inputSource.GetInput(inputName.Value);
        }

        public override void TargetHasChanged()
        {
            inputSource = MTools.VerifyInterface((Component)CacheTarget, inputSource);
        }

        protected override bool _Evaluate()
        {
            if (input == null)
            {
                // Debugging("No Input Source Found", false, null);
                return false;
            }
            else
            {
                // Debugging($"Input Found: {input != null}", input.GetValue, inputSource.transform);
                return input.GetValue;
            }
        }
    }
}
