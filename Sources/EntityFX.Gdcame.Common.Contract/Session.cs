namespace EntityFX.Gdcame.Contract.Common
{
    using System;
    
    
    public class Session
    {
        
        public Guid SessionIdentifier { get; set; }

        
        public string Login { get; set; }

        
        public string UserId { get; set; }

        
        public UserRole[] UserRoles { get; set; }

        public DateTime LastActivity { get; set; }

        //public IIdentity Identity { get; set; }
    }
}