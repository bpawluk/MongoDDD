namespace MongoDDD.Core
{
    internal class AggregateSnapshot<TAggregateState>
    {
        public string Id { get; }

        public int Version { get; }

        public TAggregateState State { get; }

        public AggregateSnapshot(string id, int version, TAggregateState state)
        {
            Id = id;
            Version = version;
            State = state;
        }
    }
}
