namespace Elfar
{
    public class Settings
    {
        public string Application
        {
            get { return application ?? "''"; }
            set { application = value; }
        }

        string application;
    }
}