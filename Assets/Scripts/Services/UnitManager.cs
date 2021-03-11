using UnityEngine;

namespace Services
{
    public class UnitManager : IGameService
    {
        public UnitController Controller { get; set; }

        public UnitMoveVisualizer Visualizer {get;set;}
    }
}