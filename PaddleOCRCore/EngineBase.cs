using System.Drawing;
using System.Runtime.InteropServices;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

namespace PaddleOCRCore
{
    /// <summary>
    /// PaddleOCR识别引擎对象
    /// </summary>
    public abstract class EngineBase 
    {
        /// <summary>
        /// PaddleOCR.dll自定义加载路径，默认为空，如果指定则需在引擎实例化前赋值。
        /// </summary>
        public static string PaddleOCRdllPath { get; set; }

        internal const string dllName = "PaddleOCR";
        [DllImport("kernel32.dll")]
        private extern static IntPtr LoadLibrary(String path);

        [DllImport(dllName, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
        internal static extern IntPtr GetError();

        /// <summary>
        /// 初始化
        /// </summary>
        public EngineBase()
        {
            //此行代码无实际意义，用于后面的JsonHelper.DeserializeObject的首次加速，首次初始化会比较慢，放在此处预热。
            var temp = JsonHelper.DeserializeObject<TextBlock>("{}");
            try
            {
                if (string.IsNullOrEmpty(PaddleOCRdllPath))
                {
                    PaddleOCRdllPath = GetDllDirectory();
                }
                if (!string.IsNullOrEmpty(PaddleOCRdllPath))
                {
                    string Envpath = Environment.GetEnvironmentVariable("path", EnvironmentVariableTarget.Process);
                    if (!string.IsNullOrEmpty(Envpath))
                    {
                        Environment.SetEnvironmentVariable("path", Envpath + ";" + PaddleOCRdllPath, EnvironmentVariableTarget.Process);
                        LoadLibrary(System.IO.Path.Combine(PaddleOCRdllPath, dllName));
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("设置自定义加载路径失败。" + e.Message);
            }
        }
        #region private
        /// <summary>
        /// 获取程序的当前路径;
        /// </summary>
        /// <returns></returns>
        private static string GetDllDirectory()
        {
            string root = GetRootDirectory();
            var fileinfos = new DirectoryInfo(root).GetFiles(dllName, SearchOption.AllDirectories);
            if (fileinfos != null && fileinfos.Length > 0)
            {
                root = fileinfos.First().DirectoryName;
            }
            return root;
        }
        /// <summary>
        /// 获取程序的当前路径;
        /// </summary>
        /// <returns></returns>
        public static string GetRootDirectory()
        {
            string root = AppDomain.CurrentDomain.BaseDirectory;
#if NET46_OR_GREATER || NETCOREAPP
            root = AppContext.BaseDirectory;
#endif
            return root;
        }

        /// <summary>
        /// Convert Image to Byte[]
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        internal protected byte[] ImageToBytes(Image image)
        {
            ImageFormat format = image.RawFormat;
            using (MemoryStream ms = new MemoryStream())
            {
                if (format.Guid == ImageFormat.Jpeg.Guid)
                {
                    image.Save(ms, ImageFormat.Jpeg);
                }
                else if (format.Guid == ImageFormat.Png.Guid)
                {
                    image.Save(ms, ImageFormat.Png);
                }
                else if (format.Guid == ImageFormat.Bmp.Guid)
                {
                    image.Save(ms, ImageFormat.Bmp);
                }
                else if (format.Guid == ImageFormat.Gif.Guid)
                {
                    image.Save(ms, ImageFormat.Gif);
                }
                else if (format.Guid == ImageFormat.Icon.Guid)
                {
                    image.Save(ms, ImageFormat.Icon);
                }
                else
                {
                    image.Save(ms, ImageFormat.Png);
                }
                byte[] buffer = new byte[ms.Length];
                ms.Seek(0, SeekOrigin.Begin);
                ms.Read(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        #endregion
        /// <summary>
        /// 释放内存
        /// </summary>
        public virtual void Dispose()
        {
        }
        /// <summary>
        /// 获取底层错误信息
        /// </summary>
        /// <returns></returns>
        public virtual string GetLastError()
        {
            string err = "";
            try
            {
                var errptr = GetError();
                if (errptr != IntPtr.Zero)
                {
                    err = Marshal.PtrToStringAnsi(errptr);
                    Marshal.FreeHGlobal(errptr);
                }
            }
            catch (Exception e)
            {
                err = e.Message;
            }
            return err;
        }

    }
}