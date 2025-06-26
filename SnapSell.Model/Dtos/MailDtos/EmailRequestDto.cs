namespace SnapSell.Domain.Dtos.MailDtos
{
    public class EmailRequestDto
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public object BodyData { get; set; }
    }
}
