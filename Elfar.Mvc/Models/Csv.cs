using System;
using System.ComponentModel;
using System.Text;

namespace Elfar.Mvc.Models
{
    class Csv
    {
        public Csv(IErrorLogProvider provider, Uri root)
        {
            this.provider = provider;
            this.root = root.Scheme + "://" + root.Authority + "/elfar/";
        }
        
        public void Load()
        {
            ErrorLogException ex = null;
            AppendHeader();
            try
            {
                foreach(var errorLog in provider.List())
                    AppendLine(errorLog);
            }
            catch(Exception e)
            {
                ex = new ErrorLogException(e);
            }
            OnComplete(new AsyncCompletedEventArgs(ex, false, this));
        }
        public void OnComplete(AsyncCompletedEventArgs e)
        {
            if(Complete != null) Complete(this, e);
        }
        public override string ToString()
        {
            return builder.ToString();
        }

        void AppendHeader()
        {
            const string header = "Host,Application,ID,Time,Type,Source,User,Code,Message,URL,XMLREF,JSONREF";
            builder.AppendLine(header);
        }
        void AppendLine(Elfar.ErrorLog errorLog)
        {
            const string format = @"{0},{1},{2},{3},{4},""{5}"",""{6}"",{7},""{8}"",{9},{10},{11}";
            builder.AppendFormat(
                format,
                errorLog.Host,
                errorLog.Application,
                errorLog.ID,
                errorLog.Time,
                errorLog.Type,
                errorLog.Source,
                errorLog.User,
                errorLog.Code,
                errorLog.Message,
                string.Concat(root, errorLog.ID),
                string.Concat(root, errorLog.ID, "/Xml"),
                string.Concat(root, errorLog.ID, "/Json"));
            builder.AppendLine();
        }

        public event AsyncCompletedEventHandler Complete;
        
        readonly StringBuilder builder = new StringBuilder();
        readonly IErrorLogProvider provider;
        readonly string root;
    }
}