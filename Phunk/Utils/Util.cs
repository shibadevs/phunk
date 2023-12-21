using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Phunk.Utils
{
    public class Util
    {
        public static string GetPackageValue(string filePath, string keyword)
        {
            try
            {
                string fileContent = File.ReadAllText(filePath);

                // Use regular expression to find the package value
                string pattern = $"{keyword}\"([^\"]+)\"";
                Match match = Regex.Match(fileContent, pattern);

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            return null;
        }

        public static bool ReplaceTextInFile(string filePath, string searchText, string replaceText)
        {
            try
            {
                string fileContent = File.ReadAllText(filePath);

                // Replace the specified text
                string updatedContent = fileContent.Replace(searchText, replaceText);

                // Write the updated content back to the file
                File.WriteAllText(filePath, updatedContent);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public static bool RenameFolder(string folderPath, string newFolderName)
        {
            try
            {
                // Get the parent directory of the folder
                string parentDirectory = Path.GetDirectoryName(folderPath);

                // Construct the new path with the updated folder name
                string newFolderPath = Path.Combine(parentDirectory, newFolderName);

                // Rename the folder by moving it to the new path
                Directory.Move(folderPath, newFolderPath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public static bool ReplaceTextInDirectory(string directoryPath, string searchText, string replaceText)
        {
            try
            {
                // Get all files in the directory and its subdirectories
                string[] files = Directory.GetFiles(directoryPath, "*", SearchOption.AllDirectories);

                foreach (string filePath in files)
                {
                    try
                    {
                        string fileContent = File.ReadAllText(filePath);

                        // Replace the specified text in the file content
                        string updatedContent = fileContent.Replace(searchText, replaceText);

                        // Write the updated content back to the file
                        File.WriteAllText(filePath, updatedContent);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public static bool RenameFile(string filePath, string newFileName)
        {
            try
            {
                // Get the directory of the file
                string fileDirectory = Path.GetDirectoryName(filePath);

                // Construct the new path with the updated file name
                string newFilePath = Path.Combine(fileDirectory, newFileName);

                // Rename the file by moving it to the new path
                File.Move(filePath, newFilePath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public static bool ReplaceKeywordInFileNames(string directoryPath, string keywordToReplace, string newText)
        {
            try
            {
                if (Directory.Exists(directoryPath))
                {
                    // Get all files in the directory
                    string[] files = Directory.GetFiles(directoryPath);

                    foreach (string filePath in files)
                    {
                        try
                        {
                            // Get the file name without the extension
                            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);

                            // Check if the keyword is present in the file name
                            if (fileNameWithoutExtension.Contains(keywordToReplace))
                            {
                                // Replace the keyword in the file name
                                string newFileName = fileNameWithoutExtension.Replace(keywordToReplace, newText);

                                // Construct the new file path with the updated file name
                                string newFilePath = Path.Combine(directoryPath, newFileName + Path.GetExtension(filePath));

                                // Rename the file by moving it to the new path
                                File.Move(filePath, newFilePath);
                            }
                        }
                        catch (Exception ex)
                        {
                            //Console.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    //Console.WriteLine($"Directory '{directoryPath}' not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }

        public static bool MoveFolder(string sourceFolderPath, string destinationFolderPath)
        {
            try
            {
                if (Directory.Exists(sourceFolderPath))
                {
                    // Create the destination directory if it doesn't exist
                    if (!Directory.Exists(destinationFolderPath))
                    {
                        Directory.CreateDirectory(destinationFolderPath);
                    }

                    // Get the name of the source folder
                    string sourceFolderName = new DirectoryInfo(sourceFolderPath).Name;

                    // Combine the destination path with the source folder name
                    string destinationPath = Path.Combine(destinationFolderPath, sourceFolderName);

                    // Move the folder to the new location
                    Directory.Move(sourceFolderPath, destinationPath);

                    return true;
                }
                else
                {
                    Console.WriteLine($"Source folder '{sourceFolderPath}' not found.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}
