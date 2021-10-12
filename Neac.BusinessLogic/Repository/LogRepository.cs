using Microsoft.Extensions.Configuration;
using Neac.BusinessLogic.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Neac.BusinessLogic.Repository
{
    public class LogRepository : ILogRepository
    {
        private readonly IConfiguration _configuration;
        private readonly string _logPath;
        public LogRepository(IConfiguration configuration)
        {
            _configuration = configuration;
            _logPath = (_configuration.GetSection("LogPath").Value).Replace("{LogDate}", DateTime.Now.ToString("ddMMyyyy"));
        }
        public async Task ErrorAsync(string message)
        {
            await WriteFileAsync($"\nError: {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} : {message}");
        }
        public async Task ErrorAsync(Exception ex)
        {
            await WriteFileAsync($"\nError: {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} : {ex.ToString()}");
        }

        public async Task InfoAsync(string message)
        {
            await WriteFileAsync($"\nInfo: {DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss")} : {message}");
        }

        private async Task WriteFileAsync(string message)
        {
            if (!Directory.Exists(Path.GetDirectoryName(_logPath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(_logPath));
            }
            if (!File.Exists(_logPath))
            {
                await File.WriteAllTextAsync(_logPath, null);
            }
            await File.AppendAllTextAsync(_logPath, message);
        }
    }
}
