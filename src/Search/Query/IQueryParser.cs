namespace Disk.Search.Query
{
    public interface IQueryParser
    {
        IQuery Parse(string expression);
    }
}