namespace Common.General.Dto.Result
{
   public class ValidationError
    {
        public string Message { get; set; }

        public string MessageType { get; set; }
       

        public ValidationError(string message)
        {
            Message = message;
        }
       
        public ValidationError(string message, string messageType)
        {
            Message = message;
            MessageType = messageType;
        }
        
    }
}
