namespace Common.General.Dto.Result
{
   public class Error
    {
        public string Message { get; set; }

        public string MessageType { get; set; }
       

        public Error(string message)
        {
            Message = message;
        }
       
        public Error(string message, string messageType)
        {
            Message = message;
            MessageType = messageType;
        }
        
    }
}
