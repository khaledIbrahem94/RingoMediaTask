namespace RingoMedia.MVC.Models.ViewModel;

public class CreateDepartmentViewModel
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public IFormFile? LogoFile { get; set; }
    public string? LogoPath { get; set; }
    public int? ParentDepartmentId { get; set; }
}
