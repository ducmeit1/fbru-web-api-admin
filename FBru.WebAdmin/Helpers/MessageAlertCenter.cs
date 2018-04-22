namespace FBru.WebAdmin.Helpers
{
    public enum MessageAlertType
    {
        Success,
        BadRequest,
        Invalid,
        Custom,
        ServerError
    }

    public class MessageAlertCenter
    {
        public bool Status { get; set; }
        public bool BadRequest { get; set; }
        public string Message { get; set; }
        public string ReturnUrl { get; set; }

        public static MessageAlertCenter GetMessageAlert(MessageAlertType type, string returnUrl = null,
            string message = null)
        {
            MessageAlertCenter messageAlert = null;
            switch (type)
            {
                case MessageAlertType.Success:
                    messageAlert = new MessageAlertCenter
                    {
                        Status = true,
                        BadRequest = false,
                        Message = message,
                        ReturnUrl = returnUrl
                    };
                    break;
                case MessageAlertType.BadRequest:
                    messageAlert = new MessageAlertCenter
                    {
                        Status = false,
                        BadRequest = true,
                        Message = "Bad request!",
                        ReturnUrl = returnUrl
                    };
                    break;
                case MessageAlertType.Invalid:
                    messageAlert = new MessageAlertCenter
                    {
                        Status = false,
                        BadRequest = false,
                        Message = message,
                        ReturnUrl = returnUrl
                    };
                    break;
                case MessageAlertType.ServerError:
                    messageAlert = new MessageAlertCenter
                    {
                        Status = false,
                        BadRequest = false,
                        Message = "Server error!",
                        ReturnUrl = returnUrl
                    };
                    break;
                case MessageAlertType.Custom:
                    messageAlert = new MessageAlertCenter();
                    break;
            }
            return messageAlert;
        }
    }
}