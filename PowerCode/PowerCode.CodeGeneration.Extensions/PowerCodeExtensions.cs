using PowerCode.CodeGeneration.Core;

using System;
using System.Diagnostics;

namespace PowerCode.CodeGeneration.Extensions
{
    public static class PowerCodeExtensions
    {
        public static string ExecutePowerCode(this string powerCode)
        {
            string result = string.Empty;

            if (string.IsNullOrEmpty(powerCode))
            {
                return result;
            }
            //Execute
            var generator = new PowerCodeGenerator();
            try
            {
                result = generator.Generate(powerCode);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
            //Return
            return result;

        }
    }
}
