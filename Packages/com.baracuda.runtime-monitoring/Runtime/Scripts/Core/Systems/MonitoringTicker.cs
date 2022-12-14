// Copyright (c) 2022 Jonathan Lang

using Baracuda.Monitoring.Interfaces;
using Baracuda.Monitoring.Types;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Baracuda.Monitoring.Systems
{
    internal class MonitoringTicker : IMonitoringTicker
    {
        /*
         * Properties
         */

        public bool ValidationTickEnabled { get; set; } = true;

        //--------------------------------------------------------------------------------------------------------------

        private readonly List<IMonitorUnit> _activeTickReceiver = new List<IMonitorUnit>(64);
        private event Action ValidationTick;

        private static float updateTimer;
        private static float validationTimer;
        private static bool tickEnabled;

        //--------------------------------------------------------------------------------------------------------------

        internal MonitoringTicker(IMonitoringManager monitoringManager)
        {
            monitoringManager.ProfilingCompleted += MonitoringEventsOnProfilingCompleted;
            this.RegisterMonitor();
        }

        private void MonitoringEventsOnProfilingCompleted(IReadOnlyList<IMonitorUnit> staticUnits, IReadOnlyList<IMonitorUnit> instanceUnits)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                throw new Exception("Application must be in playmode!");
            }
#endif

            var sceneHook = new GameObject("Monitoring Scene Hook").AddComponent<SceneHook>();

            UnityEngine.Object.DontDestroyOnLoad(sceneHook);

            sceneHook.gameObject.hideFlags = MonitoringSystems.Settings.ShowRuntimeMonitoringObject
                ? HideFlags.None
                : HideFlags.HideInHierarchy;

            sceneHook.LateUpdateEvent += Tick;

            tickEnabled = MonitoringSystems.UI.Visible;

            MonitoringSystems.UI.VisibleStateChanged += visible =>
            {
                tickEnabled = visible;
                if (!visible)
                {
                    return;
                }

                UpdateTick();
                ValidationTick?.Invoke();
            };
        }
        private void Tick(float deltaTime)
        {
            if (!tickEnabled)
            {
                return;
            }

            updateTimer += deltaTime;
            if (updateTimer > .05f)
            {
                updateTimer = 0;
                UpdateTick();
            }

            validationTimer += deltaTime;
            if (validationTimer > .1f)
            {
                validationTimer = 0;
                if (ValidationTickEnabled)
                {
                    ValidationTick?.Invoke();
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void UpdateTick()
        {
            for (var i = 0; i < _activeTickReceiver.Count; i++)
            {
                _activeTickReceiver[i].Refresh();
            }
        }

        public void AddUpdateTicker(IMonitorUnit unit)
        {
            _activeTickReceiver.Add(unit);
        }

        public void RemoveUpdateTicker(IMonitorUnit unit)
        {
            _activeTickReceiver.Remove(unit);
        }

        public void AddValidationTicker(Action tickAction)
        {
            ValidationTick += tickAction;
        }

        public void RemoveValidationTicker(Action tickAction)
        {
            ValidationTick -= tickAction;
        }
    }
}
