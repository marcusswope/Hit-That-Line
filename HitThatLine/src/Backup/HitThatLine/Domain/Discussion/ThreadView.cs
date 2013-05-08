namespace HitThatLine.Domain.Discussion
{
    public class ThreadView
    {
        public string IPAddress { get; private set; }
        public string Username { get; private set; }
        public string DiscussionThreadId { get; private set; }

        public ThreadView(string ip, string username, string discussionId)
        {
            IPAddress = ip;
            Username = username;
            DiscussionThreadId = discussionId;
        }
    }
}