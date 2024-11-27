using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lotus_Spor.Services
{
    public static class UserSession
    {
        public static bool IsLoggedIn { get; set; } = false;
        public static string UserName { get; set; }
        public static int UserId { get; set; }
        // Gerekirse daha fazla kullanıcı bilgisi ekleyin
    }
}
