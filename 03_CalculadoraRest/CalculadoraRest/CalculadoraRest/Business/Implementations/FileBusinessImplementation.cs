using CalculadoraRest.Data.VO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace CalculadoraRest.Business.Implementations
{
    public class FileBusinessImplementation : IFileBusiness
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;

        public FileBusinessImplementation(IHttpContextAccessor context)
        {
            _context = context;

            _basePath = Directory.GetCurrentDirectory() + "\\UploadDir\\";
        }

        public async Task<FileDetailVO> SaveFileToDisk(IFormFile file)
        {
            FileDetailVO fileDetail = new FileDetailVO();
            var fileType = Path.GetExtension(file.FileName);
            //Pega o endereço local, com porta e etc, endereco base do servidor
            var baseUrl = _context.HttpContext.Request.Host;

            if (fileType.ToLower() == ".pdf"
                || fileType.ToLower() == ".jpg"
                || fileType.ToLower() == ".png"
                || fileType.ToLower() == ".jpeg")
            {
                var docName = Path.GetFileName(file.FileName);
                if (file != null && file.Length > 0)
                {
                    var destination = Path.Combine(_basePath, "", docName);
                    fileDetail.DocumentName = docName;
                    fileDetail.DocType = fileType;
                    fileDetail.DocumentName = Path.Combine(baseUrl + "/api/file/v1/" + fileDetail.DocumentName);

                    using var stream = new FileStream(destination, FileMode.Create);
                    await file.CopyToAsync(stream);
                }
            }
            return fileDetail;
        }

        public async Task<List<FileDetailVO>> SaveFileToDisk(IList<IFormFile> files)
        {
            var list = new List<FileDetailVO>();
            foreach (var file in files)
            {
                list.Add(await SaveFileToDisk(file));
            }
           return list;
        }

        public byte[] GetFile(string filename)
        {
            var filePath = _basePath + filename;
            return File.ReadAllBytes(filePath);
        }
    }
}
