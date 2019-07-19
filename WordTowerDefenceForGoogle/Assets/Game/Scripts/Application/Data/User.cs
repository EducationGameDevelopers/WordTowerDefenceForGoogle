public class User
{
    private string username;
    private string password;
    private string telNumber;
    public User()
    {
        
    }
    public User(string username, string password, string telNumber)
    {
        this.username = username;
        this.password = password;
        this.telNumber = telNumber;
    }
    public string Username
    {
        get { return username; }
        set { this.username = value; }
    }
    public string Password
    {
        get { return password; }
        set { password = value; }
    }
    public string TelNumber
    {
        get { return telNumber; }
        set { telNumber = value; }
    }

}

