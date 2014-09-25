namespace Elfar.IO
{
    public class Settings : Elfar.Settings
    {
        public string FilePath
        {
            get { return filePath ?? (filePath = this["FilePath"]); }
            set { filePath = value; }
        }

        string filePath;
    }
}