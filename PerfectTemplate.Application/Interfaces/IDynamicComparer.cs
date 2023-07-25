namespace PerfectTemplate.Application.Interfaces
{
    public interface IDynamicComparer<T>
    {
        int Compare(object x, object y, string propertyName, bool ascending);
    }
}
