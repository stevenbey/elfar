namespace Elfar.IO
{
    public class Settings : Elfar.Settings
    {
        public string FilePath
        {
            get { return filePath ?? (filePath = GetAppSetting("FilePath")); }
            set { filePath = value; }
        }

        string filePath;
    }
}