using Microsoft.AspNetCore.Mvc;
using System.Drawing.Text;
namespace Aristino.Helper
{
    public class UploadImage
    {
        private readonly IWebHostEnvironment _environment;
        public  UploadImage(IWebHostEnvironment environment)
        {
            _environment = environment;
        }
        public async Task<(string,string)> Upload(string Name,List<IFormFile> file,string ControllerName)
        {
            var createGalleryList = new List<string>();
            foreach (var item in file)
            {
                createGalleryList.Add(item.FileName);
            }
            var ProductGalleryString = String.Join(",", createGalleryList);
            var fileName = file[0].FileName;
            var FolderPath = Path.Combine(_environment.WebRootPath, @"uploads\"+ControllerName, Name);
            Directory.CreateDirectory(FolderPath);
            foreach (var item in file)
            {
                var filePath = Path.Combine(FolderPath, item.FileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                await item.CopyToAsync(fileStream);
            }
            return (fileName,ProductGalleryString);
        }
        public bool CheckDirExist(string Name,string ControllerName)
        {
            var folderPath = Path.Combine(_environment.WebRootPath, @"uploads\"+ControllerName, Name);
            if (Directory.Exists(folderPath))
                return true;
            return false;
        }
        public void DeleteUploadFile(string Name,string ControllerName)
        {
            var productFolderPath = Path.Combine(_environment.WebRootPath, @"uploads\" + ControllerName, Name);
            Directory.Delete(productFolderPath, true);
        }
        public bool CheckFileExtension(List<IFormFile> Image)
        {
            var FileExtension = "";
            int count = 0;
            var SupportedExtension = new List<string>()
            {
                ".jpg",
                ".png",
                ".gif",
                ".jpeg",
            };
            foreach(var image in Image)
            {
                foreach(var item in SupportedExtension)
                {
                    FileExtension = Path.GetExtension(image.FileName);
                    if (FileExtension == item)
                        break;
                    count++;
                }
                if (count == SupportedExtension.Count())
                    return true;
                count = 0;
            }
            return false;
        }
        public bool CheckFileSize(List<IFormFile> Image)
        { 
            foreach(var image in Image)
            {
                if (image.Length > 30000000)
                    return true;
            }
            return false;
        }
    }
}
