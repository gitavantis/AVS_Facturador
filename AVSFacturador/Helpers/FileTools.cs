using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.IsolatedStorage;
using System.Reflection;

namespace AVSFacturador.Helpers
{
    public static class FileTools
    {
        public static byte[] ReadResourcesFile(string fullresourcename)
        {
            try
            {
                byte[] regreso = null;
                var temp = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();

                using (Stream s = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream(fullresourcename))
                {
                    int length = (int)s.Length;         // get file length
                    byte[] buffer = new byte[length];   // create buffer
                    int count;                          // actual number of bytes read
                    int sum = 0;                        // total number of bytes read

                    // read until Read method returns 0 (end of the stream has been reached)
                    while ((count = s.Read(buffer, sum, length - sum)) > 0)
                        sum += count;  // sum is a buffer offset for next reading

                    regreso = buffer;
                }

                return regreso;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string ByteArrayToIsolatedFile(string filename, byte[] bytearray)
        {
            try
            {
                //IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetMachineStoreForAssembly();
                IsolatedStorageFileStream filestream = new IsolatedStorageFileStream(filename, FileMode.Create, isoStore);
                filestream.Write(bytearray, 0, bytearray.Length);
                filestream.Close();

                string path = filestream.GetType().GetField("m_FullPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(filestream).ToString();
                return path;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static string isFileinIsolatedFile(string filename)
        {
            //IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetMachineStoreForAssembly();
            IsolatedStorageFileStream filestream = new IsolatedStorageFileStream(filename, FileMode.Open, isoStore);
            string path = filestream.GetType().GetField("m_FullPath", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(filestream).ToString();
            filestream.Close();
            return path;
        }

        public static string GetIsolatedFullFilePath(string filename)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetMachineStoreForAssembly();
            if (isoStore.FileExists(filename))
            {
                var field = isoStore.GetType().GetProperty("RootDirectory", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);
                var path = (String)field.GetValue(isoStore, null);
                return path + filename;
            }
            else
                throw new FileNotFoundException("El archivo " + filename + " no existe.");
        }

        public static bool FileExistsInIsolatedFile(string filename)
        {
            try
            {
                //IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetMachineStoreForAssembly();
                if (isoStore.FileExists(filename))
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static byte[] ReadFileIsolatedStorage(string filename)
        {
            try
            {
                byte[] buffer;
                IsolatedStorageFile isoStore = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);
                IsolatedStorageFileStream filestream = new IsolatedStorageFileStream(filename, FileMode.Open, isoStore);

                try
                {
                    int length = (int)filestream.Length;  // get file length
                    buffer = new byte[length];            // create buffer
                    int count;                            // actual number of bytes read
                    int sum = 0;                          // total number of bytes read

                    // read until Read method returns 0 (end of the stream has been reached)
                    while ((count = filestream.Read(buffer, sum, length - sum)) > 0)
                        sum += count;  // sum is a buffer offset for next reading
                }
                finally
                {
                    filestream.Close();
                }

                return buffer;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public static void ByteArrayToFile(string _FileName, byte[] _ByteArray)
        {
            try
            {
                System.IO.FileStream _FileStream = new System.IO.FileStream(_FileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                _FileStream.Write(_ByteArray, 0, _ByteArray.Length);
                _FileStream.Close();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public static bool RemoteFromIsolateSotrage(string filename)
        {
            IsolatedStorageFile isoStore = IsolatedStorageFile.GetMachineStoreForAssembly();
            if (!isoStore.FileExists(filename))
                return false;

            isoStore.Remove();
            return true;
        }
    }
}