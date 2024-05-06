namespace CircuitsUc.Application.Models.AuthDTO
{
    public class RegistrationResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
       
    }

}
