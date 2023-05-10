namespace PathFinder.Geom
{
    public interface IEdge : IEquatable<IEdge>
    {
        public int Start { get; init; }
        public int End { get; init; }
        public float Cost { get; init; }
        public bool Directed { get; init; }

        public int GetHashCodeCore();

        public string GetEdgeType();

        public IEdge CreateReversed();
    }

    public abstract class BaseEdge : IEdge
    {
        public abstract int Start { get; init; }
        public abstract int End { get; init; }
        public abstract float Cost { get; init; }
        public abstract bool Directed { get; init; }

        public abstract int GetHashCodeCore();

        public abstract IEdge CreateReversed();

        public string GetEdgeType() => GetType().Name;

        public override string ToString()
        {
            if (Cost == 1) return $"({Start} -> {End})";
            return $"({Start} -> {End} : Cost={Cost})";
        }

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as IEdge);
        }

        public bool Equals(IEdge? other)
        {
            if (other == null) return false;
            return this.GetHashCodeCore() == other.GetHashCodeCore();
        }
    }

    public class NonDirectionalEdge : BaseEdge, IParsableDict<NonDirectionalEdge>
    {
        public override int Start { get; init; }
        public override int End { get; init; }
        public override float Cost { get; init; }
        public override bool Directed { get; init; }

        public NonDirectionalEdge(int start, int end, float cost=1.0f)
        {
            Start = start;
            End = end;
            Cost = cost;
            Directed = false;
        }

        public override int GetHashCodeCore()
        {
            var small = Math.Min(Start, End);
            var large = Math.Max(Start, End);
            return Tuple.Create(small, large, Cost).GetHashCode();
        }

        public override IEdge CreateReversed()
        {
            return new NonDirectionalEdge(End, Start, Cost);
        }

        public static NonDirectionalEdge Parse(Dictionary<string, dynamic?> dict) => new(dict["Start"], dict["End"], dict["Cost"]);
    }
}
