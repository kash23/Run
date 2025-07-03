using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Run.Models;

namespace Run.Services
{
    public static class UserService
    {
        private static UserData _currentUser;

        public static async Task LoadUserAsync(string phone)
        {
            var firebaseService = new FirebaseService();
            _currentUser = await firebaseService.GetUserAsync(phone);
        }

        public static UserData CurrentUser => _currentUser;
    }
}
