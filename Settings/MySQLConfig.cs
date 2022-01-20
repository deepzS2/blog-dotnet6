namespace Blog.Settings
{
  public class MySQLConfiguration
  {
    public string? Host { get; set; }
    public int Port { get; set; }
    public string? Database { get; set; }
    public string? User { get; set; }
    public string? Password { get; set; }

    public string ConnectionString => $"server={Host};user id={User};password={Password};port={Port};database={Database};";
  }
}