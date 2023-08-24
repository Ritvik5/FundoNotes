using System;
using System.Collections.Generic;
using System.Text;

namespace RepoLayer.Entities
{
    public class LoginResultModel
    {
        public string Token { get; set; }
        public UserEntity User { get; set; }
    }
}
