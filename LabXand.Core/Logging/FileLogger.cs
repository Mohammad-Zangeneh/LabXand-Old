using System;
using System.Configuration;
using System.IO;

namespace LabXand.Core.Logging
{
    public class FileLogger
    {
        public void Log(string message)
        {
            Log(ConfigurationManager.AppSettings["LogPath"], new DefaultLogPathCreator());
        }

        public void Log(string message, IPathCreator pathCreator)
        {
            Log(pathCreator.Directory, pathCreator.FileName, pathCreator.FileExtension, message);
        }

        public void Log(string directoryPath, string fileName, string fileExtension, string message)
        {

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            File.AppendAllText(string.Format(@"{0}\{1}.{2}", directoryPath, fileName, fileExtension), message);
        }
    }

    public interface IPathCreator
    {
        string Directory { get; }
        string FileName { get; }
        string FileExtension { get; }
    }

    public class DefaultLogPathCreator : IPathCreator
    {
        public string FileExtension
        {
            get
            {
                return "txt";
            }
        }

        public string FileName
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }

        public string Directory
        {
            get
            {
                string logPath = ConfigurationManager.AppSettings["LogPath"];
                logPath = string.IsNullOrEmpty(logPath) ? @"C:\LabXand\Log\" : logPath;
                return logPath;
            }
        }
    }

    public class DateSeperatedLogpathCreator : IPathCreator
    {
        string _baseDirectory;
        string _fileExtension;
        string _fileName;
        public DateSeperatedLogpathCreator(string fileName, string fileExtension) : 
            this(fileName, fileExtension, ConfigurationManager.AppSettings["LogPath"], false)
        { }
        public DateSeperatedLogpathCreator(string rootPath, string fileName, string fileExtension, bool combineWithDirectoryPath = true)
        {
            _baseDirectory = Path.Combine(combineWithDirectoryPath ? ConfigurationManager.AppSettings["LogPath"] : string.Empty, rootPath);
            _fileName = fileName;
            _fileExtension = fileExtension;
        }
        public string Directory
        {
            get
            {
                return Path.Combine(_baseDirectory, string.Format("{0}{1}{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
            }
        }

        public string FileExtension
        {
            get
            {
                return _fileExtension;
            }
        }

        public string FileName
        {
            get
            {
                return _fileName;
            }
        }
    }
}
