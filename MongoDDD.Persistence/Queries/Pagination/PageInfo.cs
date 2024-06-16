namespace MongoDDD.Persistence.Queries.Pagination
{
    public class PageInfo
    {
        public int Number { get; }

        public int Size { get; }

        public PageInfo(int number, int size)
        {
            Number = number;
            Size = size;
        }
    }
}
