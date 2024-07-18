using SuperBike.Api.FileStorage.DataAccess;
using SuperBike.Api.FileStorage.Entities;

namespace SuperBike.Api.FileStorage.ServiceHandler
{
    public class FileManager
    {
        private readonly DataAccessFile _dataAccess;
        public FileManager(DataAccessFile dataAccess)
        {
            _dataAccess = dataAccess;
        }

        public async Task<FileDisk> GetFile(string localStorage, string key)
        {
            var fileFromDb = await _dataAccess.GetByKey<FileDisk>(key);

            var localDisk = Path.Combine(localStorage, fileFromDb.LocalPath);

            if (File.Exists(localDisk))
            {
                fileFromDb.LocalPath = localDisk;

                return fileFromDb;
            }

            return null;
        }

        public async Task<FileDisk> GenerateLocal(string localStorage, IFormFile file)
        {
            var fileKey = Guid.NewGuid().ToString().Split('-')[4];
            var now = DateTime.Now;
            var localDisk = Path.Combine(                
                now.Year.ToString(),
                now.Month.ToString(),
                now.Day.ToString(),
                now.Hour.ToString(),
                now.Minute.ToString(),
                fileKey);

            WriteFile(Path.Combine(localStorage, localDisk), file);

            var fileDisk = new FileDisk()
            {
                FileName = file.FileName,
                ContentType = file.ContentType,
                Length = file.Length,
                Key = fileKey,
                LocalPath = localDisk
            };

            await _dataAccess.Insert(fileDisk);

            return fileDisk;
        }

        private void WriteFile(string local, IFormFile file)
        {
            if (!Path.Exists(local)) Directory.CreateDirectory(Directory.GetParent(local).FullName);

            using (var stream = new FileStream(local, FileMode.Create))
            {
                file.CopyTo(stream);
            }
        }
    }
}
