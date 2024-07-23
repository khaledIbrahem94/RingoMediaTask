using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RingoMedia.MVC.Models.DataBase;

public class Department
{
    [Key]
    public int Id { get; set; }
    [MaxLength(200)]
    public required string Name { get; set; }
    public string? Logo { get; set; }
    [ForeignKey(nameof(ParentDepartment))]
    public int? ParentDepartmentId { get; set; }
    public Department? ParentDepartment { get; set; }
    public virtual ICollection<Department> SubDepartments { get; set; } = new List<Department>();
}