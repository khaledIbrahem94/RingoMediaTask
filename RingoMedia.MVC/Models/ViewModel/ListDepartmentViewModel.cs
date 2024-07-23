using RingoMedia.MVC.Models.DataBase;

namespace RingoMedia.MVC.Models.ViewModel;

public class ListDepartmentViewModel
{
    public required List<DepratmentWithChilds> Departments { get; set; }
}

public class DepratmentWithChilds
{
    public required Department Department { get; set; }
    public List<Department>? Childs { get; set; } = new List<Department>();
}