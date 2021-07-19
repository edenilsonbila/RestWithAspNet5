using CalculadoraRest.Data.VO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculadoraRest.Business
{
    public interface IFileBusiness
    {
        public byte[] GetFile(string filename);

        public Task<FileDetailVO> SaveFileToDisk(IFormFile file);
        public Task<List<FileDetailVO>> SaveFileToDisk(IList<IFormFile> file);
    }
}
