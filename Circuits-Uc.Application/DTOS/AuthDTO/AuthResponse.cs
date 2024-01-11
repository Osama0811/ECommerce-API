namespace CircuitsUc.Application.Models.AuthDTO
{
    public class AuthResponse
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string? ImagePath { get; set; }
        public string? FileName { get; set; }  
        //public Guid? Id { get; set; } 
        public int RoleId { get; set; }

       
    }
}
