using System;
using System.IO;
using System.Text.RegularExpressions;

namespace DuplicateTool
{
    class Program
    {
        static void Main(string[] args)
        {
            // To check the length of  
            // Command line arguments   
            if (args.Length > 0)
            {
                string passendFile = args[0];
                FileAttributes attr = File.GetAttributes(@passendFile);

                //Regex Pattern for date
                string pattern = @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))";

                //Get Date
                string currentYear = DateTime.Now.Year.ToString();
                string currentMonth = DateTime.Now.Month.ToString().PadLeft(2, '0');
                string currentDay = DateTime.Now.Day.ToString().PadLeft(2, '0');
                string date = currentYear + "-" + currentMonth + "-" + currentDay;

                //File
                if (!attr.HasFlag(FileAttributes.Directory))
                {
                    string pathStripped = Path.GetDirectoryName(passendFile);

                    string fileName = Path.GetFileName(passendFile);

                    string newFilePath;

                    if (Regex.IsMatch(fileName, pattern))
                    {
                        newFilePath = pathStripped + "\\" + date + fileName.Remove(0, 10);
                    }
                    else
                    {
                        newFilePath = pathStripped + "\\" + date + "_" + fileName;
                    }

                    if (!File.Exists(@newFilePath))
                    {
                        File.Copy(passendFile, newFilePath);
                        Console.WriteLine("File Duplicated with todays date.");
                    }
                    else
                    {
                        Console.WriteLine("File already exists!");
                    }
                }
                //Folder
                else
                {
                    string folderAbove = System.IO.Directory.GetParent(passendFile).ToString();
                    string folderName = passendFile.Replace(folderAbove + "\\", "");

                    string newFolderPath;

                    if (Regex.IsMatch(folderName, pattern))
                    {
                        newFolderPath = folderAbove + "\\" + date + folderName.Remove(0, 10);
                    }
                    else
                    {
                        newFolderPath = folderAbove + "\\" + date + "_" + folderName;
                    }

                    if (!Directory.Exists(@newFolderPath))
                    {
                        CopyFolder(passendFile, newFolderPath);
                        Console.WriteLine("Folder Duplicated with todays date.");
                    }
                    else
                    {
                        Console.WriteLine("Folder already exists!");
                    }

                }


            }

            else
            {
                Console.WriteLine("No file found.");
            }
        }

        static public void CopyFolder(string sourceFolder, string destFolder)
        {
            if (!Directory.Exists(destFolder))
                Directory.CreateDirectory(destFolder);
            string[] files = Directory.GetFiles(sourceFolder);
            foreach (string file in files)
            {
                string name = Path.GetFileName(file);
                string dest = Path.Combine(destFolder, name);
                File.Copy(file, dest);
            }
            string[] folders = Directory.GetDirectories(sourceFolder);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName(folder);
                string dest = Path.Combine(destFolder, name);
                CopyFolder(folder, dest);
            }
        }
    }
}
