namespace EntityFX.Gdcame.Engine.Contract.GameEngine
{
    using System;
    using System.Runtime.Serialization;


    public class User
    { 
        public string Id { get; set; }


        public string Login { get; set; }


        public uint Role { get; set; }
    }
}