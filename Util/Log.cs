using System;
using System.IO;
using System.Text;

namespace Desktop.GrupoBW.Util
{
    public class Log
    {
        public static void GravarLog(Exception exception)
       {
            try
            {
                string LogErro = string.Format("{0}LogErro-{1}.txt", AppDomain.CurrentDomain.BaseDirectory, DateTime.Now.ToString("yyyyMMdd"));

                string Mensagem = string.Format("{0}{0}=== {1} ==={0}{2}{0}{3}{0}{4}{0}{5}",
                    Environment.NewLine, DateTime.Now, exception.Message, exception.Source, exception.InnerException, exception.StackTrace);

                byte[] LogString = Encoding.Default.GetBytes(Mensagem);
                FileStream arquivoLog = new FileStream(LogErro, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write);

                //GRAVAR LOG
                arquivoLog.Seek(0, System.IO.SeekOrigin.End);
                arquivoLog.Write(LogString, 0, LogString.Length);
                arquivoLog.Close();
                
            }
            catch { }
        }
    }
}
