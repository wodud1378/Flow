using Flow.Core;
using Flow.Core.Interfaces;
using UnityEngine;
using VContainer;

namespace Flow.Sample.GamePlay
{
    [RequireComponent(typeof(GameFlow))]
    public class GamePlayer : MonoBehaviour
    {
        [SerializeField] private GameFlow flow;
        
        private GameContext _context;
        
        private void OnValidate()
        {
            if (flow != null) return;

            if (TryGetComponent(out flow)) return;
            
            flow = gameObject.AddComponent<GameFlow>();
        }

        [Inject]
        public void InitializeDependencies(IFlowServiceProvider serviceProvider)
        {
            _context = new();
            
            flow.InitializeDependencies(_context, serviceProvider);
        }
    }
}