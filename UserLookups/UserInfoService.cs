using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;

namespace UserLookup
{
    internal class UserInfoService
    {
        private static readonly string[] _lineEndings = { "\n", "\r\n" };

        public string[] GetUserList(string userFilename)
        {
            string data = File.ReadAllText(userFilename);
            string[] users = data.Split(_lineEndings, StringSplitOptions.RemoveEmptyEntries);

            return users;
        }

        public UserInfo GetUserInfo(string userName)
        {
            UserInfo user = new() { UserName = userName };

            using (var pc = new PrincipalContext(ContextType.Domain, Environment.UserDomainName))
            {

                if (!string.IsNullOrWhiteSpace(userName))
                {
                    var userPrincipal = UserPrincipal.FindByIdentity(pc, userName);

                    if (userPrincipal != null)
                    {
                        user.Name = userPrincipal.DisplayName;
                        user.JobTitle = userPrincipal.Description;

                        string? department = ((DirectoryEntry)userPrincipal.GetUnderlyingObject())
                            ?.Properties["department"]?.Value?.ToString();

                        user.Team = department ?? string.Empty;
                    }

                }

            }

            return user;
        }

    }

}
