namespace PathFinder.Geom
{
    public interface IParsableDict<T> where T : class
    {
        public static abstract T Parse(Dictionary<string, dynamic?> dict);
    }
}
