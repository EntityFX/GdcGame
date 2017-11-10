namespace EntityFX.Gdcame.Contract.Common
{
    

    
    public class UserData
    {
        
        public string Id { get; set; }

        
        public string Login { get; set; }

        
        public string PasswordHash { get; set; }

        
        public UserRole[] UserRoles { get; set; }
    }
}