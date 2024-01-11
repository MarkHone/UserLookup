using Microsoft.Extensions.Configuration;
using UserLookup;

IConfigurationBuilder builder = new ConfigurationBuilder()
    .AddJsonFile("appSettings.json");

IConfigurationRoot configuration = builder.Build();
string? userFilename = configuration["userFilename"];

if (!string.IsNullOrEmpty(userFilename))
{
    Console.WriteLine("Loading user list...");
    var svc = new UserInfoService();
    string[] usernames = svc.GetUserList(userFilename);

    using (StreamWriter sw = new("user-info.csv"))
    {
        sw.WriteLine("UserName,Name,JobTitle,Team");

        foreach (string username in usernames)
        {
            UserInfo user = svc.GetUserInfo(username);

            if (string.IsNullOrEmpty(user.Name))
            {
                Console.WriteLine($"{user.UserName}: not found.");
                sw.WriteLine($"{user.UserName},not found,,");
            }
            else
            {
                Console.WriteLine($"{user.UserName}: {user.Name} ({user.JobTitle} in {user.Team}).");
                sw.WriteLine($"\"{user.UserName}\",\"{user.Name}\",\"{user.JobTitle}\",\"{user.Team}\"");
            }

        }

    }

}
