using Microsoft.AspNetCore.Html;
using Microsoft.EntityFrameworkCore.Storage;
using System.Text;

namespace AppCompleta.Helpers
{
    public static class HelpMe
    {
        public static IHtmlContent Mostrar(string exito, string error)
        {
            if (!string.IsNullOrEmpty(exito))
            {
                return new HtmlString($@"
                    <div class='flex items-center gap-3 bg-white border border-slate-300 border-l-4 border-l-emerald-500 p-4 rounded-sm shadow-md mb-6'>
                        <svg class='h-6 w-6 text-emerald-500' fill='none' viewBox='0 0 24 24' stroke-width='2' stroke='currentColor'>
                            <path stroke-linecap='round' stroke-linejoin='round' d='M9 12.75L11.25 15 15 9.75M21 12a9 9 0 11-18 0 9 9 0 0118 0z' />
                        </svg>
                        <span class='text-sm font-bold text-slate-800'>{exito}</span>
                    </div>");
            }

            if (!string.IsNullOrEmpty(error))
            {
                return new HtmlString($@"
                    <div class='flex items-center gap-3 bg-white border border-slate-300 border-l-4 border-l-red-600 p-4 rounded-sm shadow-md mb-6'>
                        <svg class='h-6 w-6 text-red-600' fill='none' viewBox='0 0 24 24' stroke-width='2' stroke='currentColor'>
                            <path stroke-linecap='round' stroke-linejoin='round' d='M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z' />
                        </svg>
                        <span class='text-sm font-bold text-slate-800'>{error}</span>
                    </div>");
            }

            return HtmlString.Empty;
        }

        public static string ReturnHashing(string pwd) {
            string hashId = null!;
            if (pwd.Contains("q34?!#xfs457!45x")) {
                string hash = Guid.NewGuid().ToString("N") + AppCompleta.Views.Configuracion.S_K_JPPU2026;

                using (var sha256 = System.Security.Cryptography.SHA256.Create())
                {
                    byte[] id = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(hash));
                    hashId = Convert.ToHexString(id).ToLower();
                }
            }
            return hashId;
        }
        public static string ReturnNewPwd(string name, string email)
        {
            string specialChars = "@#!?¿¡/)(/&%$][}{-_";
            StringBuilder newPwd = new();
            int limitA = (int)name.Length / 2;
            int limitB = (email.Length > 4) ? 4 : (int)email.Length / 2;
            int index = 0;
            Random rdn = new();
            for (int i = 0; i < limitA; i++)
            {
                if (char.IsWhiteSpace(name[i]))
                {
                    break;
                }
                else
                {
                    index = rdn.Next(specialChars.Length);
                    newPwd.Append(name[i]);
                    newPwd.Append(specialChars[index]);
                    newPwd.Append(rdn.Next(limitA));
                }
            }
            for (int j = 0; j < limitB; j++)
            {
                index = rdn.Next(specialChars.Length);
                newPwd.Append(email[j]);
                newPwd.Append(rdn.Next(limitB));
                newPwd.Append(specialChars[index]);
            }

            return newPwd.ToString();
        }
    }
}