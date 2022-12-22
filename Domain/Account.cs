namespace Domain;

public class Account
{
    public int IdAccount { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }
    public int IdAddress { get; set; }
    
    public bool? IsAdmin {get; set;}
    public string PictureURL {get; set;}
    public string Phone { get; set; }

    public bool matchPassword(string confirmPassword)
    {
        if (Password == confirmPassword)
        {
            return true;
        }

        return false;
    }

    public bool goodMail()
    {
        int nbDot=0;
        int nbAt=0;
        if (!Email.Contains('@') || !Email.Contains('.'))
        {
            return false;
        }
        for (int i = 0; i < Email.Length; i++)
        {
            if (Email[i] == '@')
            {
                nbAt++;
            }
            else if (Email[i] == '.')
            {
                nbDot++;
            }
        }

        if (nbAt != 1 || nbDot != 1)
        {
            return false;
        }

        if (Email.IndexOf('@') > Email.IndexOf('.'))
        {
            return false;
        }

        if (Email.IndexOf('.') == Email.Length-1)
        {
            return false;
        }

        return true;
    }
}