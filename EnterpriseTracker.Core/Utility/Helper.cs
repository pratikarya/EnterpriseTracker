using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace EnterpriseTracker.Core.Utility
{
    public static class Helper
    {
        static Helper()
        {
        }

        public static Task<Stream> GetStreamFromBytes(byte[] bytes)
        {
            //Since we need to return a Task<Stream> we will use a TaskCompletionSource>
            TaskCompletionSource<Stream> tcs = new TaskCompletionSource<Stream>();

            tcs.TrySetResult(new MemoryStream(bytes));

            return tcs.Task;
        }

        public static byte[] GetBytesFromStream(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        public static async Task<bool> HasPermission(Permission permission)
        {
            var status = await CrossPermissions.Current.CheckPermissionStatusAsync(permission);

            if (status != PermissionStatus.Granted)
            {
                var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { permission });
                status = results[permission];
            }

            return status == PermissionStatus   .Granted;
        }
    }
}
