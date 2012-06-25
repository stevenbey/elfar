namespace Elfar
{
    public interface IErrorLogMail
    {
        void Send(ErrorLog errorLog);
    }
}