using UICeo.Sorting.Models;
using UICeo.Sorting.Helpers;

namespace UICeo.Sorting.Strategy;

internal class SortBySalary : ISortBy
{
    public SortByEnum Type => SortByEnum.Salary;

    public int Compare(EmployeeController x, EmployeeController y)
    {
        var skillX = x.employeeModel.salary;
        var skillY = y.employeeModel.salary;

        return CompareFunctions.CompareFloat(skillX, skillY);
    }
}
