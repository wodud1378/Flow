using System;
using Flow.Core.Interfaces;
using Flow.Core.Model;

namespace Flow.Sample.GamePlay
{
    public class GameContext : IGameContext
    {
        public float TimeElapsed { get; }

        public IGameResult GetResult()
        {
            throw new NotImplementedException();
        }
    }
}