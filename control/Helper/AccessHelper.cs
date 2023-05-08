namespace control.Helper
{
    public static class AccessHelper
    {
        static System.Random random  = new System.Random();

        public static string GenerateAccessCode(int lenght = 9)
        {

            //We generate a random code with the given lenght but with at least given lenght.
            long new_code = random.NextInt64((long)Math.Pow(10, lenght), (long)Math.Pow(10, lenght+1));


            return Convert.ToString(new_code);
        }
        
    }
}
