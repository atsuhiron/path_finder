namespace PathFinder.Geom
{
    public interface IEdge
    {
        public uint Start { get; init; }
        public uint End { get; init; }
        public uint Cost { get; init; }
        public bool Directed { get; init; }
    }

    public abstract class BaseEdge : IEdge
    {
        public abstract uint Start { get; init; }
        public abstract uint End { get; init; }
        public abstract uint Cost { get; init; }
        public abstract bool Directed { get; init; }

        public override string ToString()
        {
            if (Cost == 1) return $"({Start} -> {End})";
            return $"({Start} -> {End} : Cost={Cost})";
        }
    }

    public class NonDirectionalEdge : BaseEdge
    {
        public override uint Start { get; init; }
        public override uint End { get; init; }
        public override uint Cost { get; init; }
        public override bool Directed { get; init; }

        public NonDirectionalEdge(uint start, uint end, uint cost=1)
        {
            Start = start;
            End = end;
            Cost = cost;
            Directed = false;
        }
    }
}
