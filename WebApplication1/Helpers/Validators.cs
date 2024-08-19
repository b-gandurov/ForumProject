using ForumProject.Resources;
using Microsoft.Data.SqlClient.Server;
using System.Net;
using System.Security.Authentication;

namespace ForumProject.Helpers
{
    public static class Validators
    {
        public static object CredentialsFormat(string credentials)
        {
            if (credentials == null || !credentials.Contains(":"))
                throw new InvalidCredentialException(string.Format(ErrorMessages.InvalidCredentialsFormat, credentials));

            string[] parts = credentials.Split(':');
            if (parts.Length != 2)
                throw new InvalidCredentialException(string.Format(ErrorMessages.InvalidCredentialsFormat, credentials));

            var username = parts[0];
            var password = parts[1];

            var credentialObject = new { Username = username, Password = password };
            return credentialObject;
        }


    }
}
