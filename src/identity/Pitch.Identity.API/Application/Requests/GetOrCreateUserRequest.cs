namespace Pitch.Identity.API.Application.Requests
{
    public class GetOrCreateUserRequest
    {
        public GetOrCreateUserRequest(string email)
        {
            Email = email;
        }

        public GetOrCreateUserRequest()
        {
                
        }

        public string Email { get; set; }
    }
}
