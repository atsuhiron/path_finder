﻿namespace PathFinder.Geom
{
    public interface INode : IEquatable<INode>
    {
        public int Index { get; init; }

        public int GetHashCodeCore();

        public string GetNodeType();
    }

    public abstract class BaseNode : INode
    {
        public abstract int Index { get; init; }

        public abstract int GetHashCodeCore();

        public abstract string GetNodeType();

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

    public class CoreNode : BaseNode
    {
        public override int Index { get; init; }

        public override string GetNodeType() => typeof(CoreNode).Name;

        public override int GetHashCodeCore()
        {
            return Index.GetHashCode();
        }

        public CoreNode(int index)
        {
            Index = index;
        }
    }
}
