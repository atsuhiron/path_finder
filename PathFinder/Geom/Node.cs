﻿namespace PathFinder.Geom
{
    public interface INode : IEquatable<INode>
    {
        public int Index { get; init; }

        public int GetHashCodeCore();

        public string GetNodeType();
    }

    public interface IXYCoordinated
    {
        public float X { get; init; }

        public float Y { get; init; }
    }

    public abstract class BaseNode : INode
    {
        public abstract int Index { get; init; }

        public abstract int GetHashCodeCore();

        public string GetNodeType() => GetType().Name;

        public override bool Equals(object? obj)
        {
            return this.Equals(obj as INode);
        }

        public bool Equals(INode? other)
        {
            if (other == null) return false;
            return this.GetHashCodeCore() == other.GetHashCodeCore();
        }

        public override int GetHashCode()
        {
            return GetHashCodeCore();
        }
    }

    public class CoreNode : BaseNode, IParsableDict<CoreNode>
    {
        public override int Index { get; init; }

        public override int GetHashCodeCore()
        {
            return Index.GetHashCode();
        }

        public CoreNode(int index)
        {
            Index = index;
        }

        public static CoreNode Parse(Dictionary<string, dynamic?> dict) => new(dict["Index"]);
    }

    public class XYNode : BaseNode, IXYCoordinated, IParsableDict<XYNode>
    {
        public override int Index { get; init; }

        public float X { get; init; }

        public float Y { get; init; }        

        public XYNode(int index, float x, float y)
        {
            Index = index;
            X = x;
            Y = y;
        }

        public override int GetHashCodeCore()
        {
            return Tuple.Create(Index, X, Y).GetHashCode();
        }

        public static XYNode Parse(Dictionary<string, dynamic?> dict) => new(dict["Index"], dict["X"], dict["Y"]);
    }
}
