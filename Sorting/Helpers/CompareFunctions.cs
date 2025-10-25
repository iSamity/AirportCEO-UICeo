using UICeo.Config;
using UICeo.Sorting.Models;

namespace UICeo.Sorting.Helpers;

internal static class CompareFunctions
{
    public static int CompareFloat(float a, float b)
    {
        var sortDirection = DefaultConfig.SortDirection.Value;


        if (sortDirection == SortDirectionEnum.Ascending)
        {
            if (a < b) return -1;
            if (a > b) return 1;
            return 0;
        }


        if (sortDirection == SortDirectionEnum.Descending)
        {
            if (a < b) return 1;
            if (a > b) return -1;
            return 0;
        }

        return 0;
    }
}
