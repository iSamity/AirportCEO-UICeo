namespace UICeo.Sorting.Models;

interface ISortBy
{
    SortByEnum Type { get; }


    int Compare(EmployeeController x, EmployeeController y);
}
