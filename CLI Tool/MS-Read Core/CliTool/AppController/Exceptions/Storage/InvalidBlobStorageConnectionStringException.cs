namespace CliTool.Exceptions.Storage
{
    class InvalidBlobStorageConnectionStringException : CliException
    {
        public InvalidBlobStorageConnectionStringException(string connectionString)
        {
            CustomMessage = "Invalid Blob Storage Connection String: " + connectionString;
        }
    }
}
