namespace InOutLogging
{
    public enum InOutLoggingWay
    {
        IncomingRequest = 0, //Server accepting Request
        OutgoingResponse = 1, //Server replying to previous Request
        OutgoingRequest = 2, //Server sending Request to a Backend
        IncomingResponse = 3, //Server receiving Response from a Backend
    }
}
