using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;

namespace LabXand.Logging.Core
{
    public class FileLogger : ILogger
    {
        public int Log(ApiLogEntry logEntry)
        {
            Log(JsonConvert.SerializeObject(logEntry));
            return 1;
        }

        public void Log(string message)
        {
            if (!(string.IsNullOrWhiteSpace(message) || message.Equals("{}")))
                Log(message, new DefaultLogPathCreator());
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

    public class ExceptionLogPathCreator : IPathCreator
    {
        private readonly Exception _exception;
        public ExceptionLogPathCreator(Exception exception)
        {
            _exception = exception;
        }
        public string Directory => string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["LogPath"])? @"C:\LabXand\Log\Exceptions\": $"{ConfigurationManager.AppSettings["LogPath"]}Exceptions\\";

        public string FileName => $"{_exception.GetType().Name}_{Guid.NewGuid()}";

        public string FileExtension => "txt";
    }

    public class DateSeperatedLogpathCreator : IPathCreator
    {
        string _baseDirectory;
        string _fileExtension;
        string _fileName;
        string _supplementaryPath;
        public DateSeperatedLogpathCreator(string fileName, string fileExtension, string supplementaryPath, bool combineWithDirectoryPath) :
            this(fileName, fileExtension, ConfigurationManager.AppSettings["LogPath"], supplementaryPath, combineWithDirectoryPath)
        { }
        public DateSeperatedLogpathCreator(string fileName, string fileExtension, string supplementaryPath) :
            this(fileName, fileExtension, ConfigurationManager.AppSettings["LogPath"], supplementaryPath, false)
        { }
        public DateSeperatedLogpathCreator(string fileName, string fileExtension) :
            this(fileName, fileExtension, string.Empty)
        { }
        public DateSeperatedLogpathCreator(string rootPath, string fileName, string fileExtension, string supplementaryPath, bool combineWithDirectoryPath = true)
        {
            _supplementaryPath = supplementaryPath;
            _baseDirectory = Path.Combine(combineWithDirectoryPath ? ConfigurationManager.AppSettings["LogPath"] : string.Empty, rootPath);
            _fileName = fileName;
            _fileExtension = fileExtension;
        }
        public string Directory
        {
            get
            {
                return Path.Combine(_baseDirectory, string.Format("{0}{1}{2}", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day), _supplementaryPath);
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
