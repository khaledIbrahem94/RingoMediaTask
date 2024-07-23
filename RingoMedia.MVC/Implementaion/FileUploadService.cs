using RingoMedia.MVC.Interface;

namespace RingoMedia.MVC.Implementaion;

public class FileUploadService : IFileUploadService
{
    private readonly IWebHostEnvironment _hostEnvironment;

    public FileUploadService(IWebHostEnvironment hostEnvironment)
    {
        _hostEnvironment = hostEnvironment;
    }

    public string UploadFile(IFormFile file, string oldImage)
    {
        string uniqueFileName = "";
        string saveFolder = "images/DepartmentLogos";
        DeleteImage(oldImage);
        if (file != null)
        {
            string uploadsFolder = Path.Combine(_hostEnvironment.WebRootPath, saveFolder);
            uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }
        }

        return $"/{saveFolder}/{uniqueFileName}";
    }

    public void DeleteImage(string oldImage)
    {
        if (!string.IsNullOrEmpty(oldImage))
        {
            oldImage = oldImage.Substring(1);
            string path = Path.Combine(_hostEnvironment.WebRootPath, oldImage);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
    }
}