namespace control.Helper
{
    public class AccessHelper
    {
        string GenerateAccessCode(int lenght = 9)
        {
            System.Random random = new System.Random();

            long new_code = random.NextInt64((long)Math.Pow(10, lenght), (long)Math.Pow(10, lenght+1));

            return Convert.ToString(new_code);
        }
        
    }
}
