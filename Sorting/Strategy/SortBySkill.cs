using UICeo.Sorting.Models;
using UICeo.Sorting.Helpers;

namespace UICeo.Sorting.Strategy;

internal class SortBySkill : ISortBy
{
    public SortByEnum Type => SortByEnum.Skill;

    public int Compare(EmployeeController x, EmployeeController y)
    {
        var skillX = x.employeeModel.skill;
        var skillY = y.employeeModel.skill;

        return CompareFunctions.CompareFloat(skillX, skillY);
    }
}
