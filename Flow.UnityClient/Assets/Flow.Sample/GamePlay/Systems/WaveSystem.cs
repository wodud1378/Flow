using Flow.Sample.GamePlay.Systems.Base;
using VContainer;

namespace Flow.Sample.GamePlay.Systems
{
    public class WaveSystem : BaseUpdateSystem
    {
        private readonly GameContext _context;

        [Inject]
        public WaveSystem(GameContext context)
        {
            _context = context;
        }
        
        protected override void OnUpdate(float deltaTime)
        {
        }
    }
}