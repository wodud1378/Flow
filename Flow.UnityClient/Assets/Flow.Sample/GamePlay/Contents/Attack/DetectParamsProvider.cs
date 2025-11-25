using System;
using Flow.Sample.Data.StaticData.Attack;
using Flow.Sample.Data.StaticData.Targeting;
using Flow.Sample.GamePlay.Components;
using Flow.Sample.GamePlay.Systems.Models;
using UnityEngine;

namespace Flow.Sample.GamePlay.Contents.Attack
{
    public class DetectParamsProvider
    {
        public IDetectParams Provide(CombatantComponent owner, TargetingData data)
        {
            var point = owner.transform.position;
            
            // move to field if needed.
            var filter = new ContactFilter2D
            {
                useLayerMask = true,
                layerMask = data.targetLayers,
            };

            return data switch
            {
                CircleTargetingData circle => circle is ArcTargetingData arc
                    ? new ArcParams(
                        arc.angle,
                        arc.radius,
                        owner.Forward,
                        point,
                        filter
                    )
                    : new CircleParams(
                        circle.radius,
                        point,
                        filter
                    ),
                BoxTargetingData box => new BoxParams(
                    box.size,
                    box.angle,
                    point,
                    filter
                ),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}