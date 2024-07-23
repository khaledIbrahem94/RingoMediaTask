namespace RingoMedia.MVC.Interface;

public interface IFileUploadService
{
    string UploadFile(IFormFile file, string oldImage);
    void DeleteImage(string image);
}
