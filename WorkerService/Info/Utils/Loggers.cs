namespace Info.Utils;

public class Loggers
{
    private string _methodName = string.Empty;

    private static string ErrorLineNumber(Exception ex)
    {
        string returnStr;
        try
        {
            var line = Convert.ToInt32(ex.StackTrace?[ex.StackTrace.LastIndexOf(' ')..]);
            returnStr = line.ToString();
        }
        catch(Exception) {
            returnStr = string.Empty;
        }
        return returnStr;
    }

    private static string ErrorMessage(string methodName, Exception exception)
    {
        var message = methodName + " - " + "Line No:" + ErrorLineNumber(exception) + " - " + exception.Message;
        return message;
    }

    public void CreateLogs(string errorMessage)
    {
        try
        {
            Console.WriteLine(DateTimeOffset.Now + " " + errorMessage);

            var fileDir = AppDomain.CurrentDomain.BaseDirectory + "\\ErrorLogs\\" + DateTime.Now.ToString("yyyyMMdd");

            var fileName = fileDir + "/" + "\\Error_Logs.txt";

            // check if the file name already exists and if not , create a new file name
            if (!Directory.Exists(fileDir))
            {
                Directory.CreateDirectory(fileDir);
            }

            if (File.Exists(fileName) == false)
            {
                var fp = File.CreateText(fileName);
                fp.Flush();
                fp.Close();
            }
            // Go ahead and write these file Logs
            //SysMessage = Type1;

            var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

            var streamWriter = new StreamWriter(fs);


            streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            streamWriter.WriteLine(DateTime.Now + " " + errorMessage);
            streamWriter.Close();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public void LogMethodsErrorDetails(string method, Exception exception, int hasMode, int mode)
    {
        try
        {
            if (hasMode == 1)
            {
                _methodName = method + "(" + mode + ")";
            }
            else
            {
                _methodName = method;
            }

            CreateLogs(ErrorMessage(_methodName, exception));
        }
        catch (Exception ex)
        {
            _methodName = System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.Name;
            CreateLogs(ErrorMessage(_methodName, ex));
        }
    }
}