namespace SqlrFixTest
{
    public class UnitTest1
    {
        /// <summary>
        /// Combines all text files in the specified directory into a single SQLR file.
        /// </summary>
        /// <param name="directoryPath">The path to the directory containing the SQLR files.</param>
        /// <param name="outputFileName">The name of the combined output text file.</param>
        [Theory]
        [InlineData("C:\\repos\\_squealerOutputTest\\peach", "_orginal.sqlr")]
        public void CombineTextFilesIntoSingleFile(string directoryPath, string outputFileName)
        {
            // Specify the output file path
            var outputFilePath = Path.Combine(directoryPath, outputFileName);

            // Get all text files in the directory
            var textFiles = Directory.GetFiles(directoryPath, "*.txt");

            using (var outputStream = new StreamWriter(outputFilePath))
            {
                foreach (var file in textFiles)
                {
                    using (var inputStream = new StreamReader(file))
                    {
                        outputStream.WriteLine(inputStream.ReadToEnd());
                    }
                }
            }

            // Optional: Verify the output file content (depends on your requirements)
            var combinedContent = File.ReadAllText(outputFilePath);
            Assert.NotEmpty(combinedContent);
        }
    }
}