using System;
using Flow.Sample.Logic.Models;

namespace Flow.Sample.GamePlay.Events
{
    public class GameEvents
    {
        public event Action<Damaged> OnDamaged;
    }
}