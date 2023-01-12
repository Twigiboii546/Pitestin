using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace Bigdecimal
{
    struct BigDecimal
    {
        public int precision;
        int Power;
        BigInteger Mantissa;
        public BigDecimal(BigInteger mantissa, int power, int precision)
        {
            while (mantissa % 10 == 0)
            {
                mantissa /= 10;
                power++;
            }
            Mantissa = mantissa;
            Power = power;
            this.precision = precision;
            Truncate();
        }
        public void ChangePrecision(int precision)
        {
            this.precision = precision;
            Truncate();
        }
        public void Truncate()
        {
            while(Mantissa > BigInteger.Pow(10, precision)) 
            {
                Mantissa /= 10;
                Power++;
            }
        }

        public static BigDecimal operator +(BigDecimal a, BigDecimal b)
        {
            int minPower = a.Power >= b.Power ? b.Power : a.Power;
            int highestprecision = a.precision >= b.precision ? a.precision : b.precision;
            return new BigDecimal(a.Mantissa * BigInteger.Pow(10, a.Power - minPower) + b.Mantissa * BigInteger.Pow(10, b.Power - minPower), minPower, highestprecision);
            
        }
        public static BigDecimal operator -(BigDecimal a)
        {
            return new BigDecimal(-a.Mantissa, a.Power, a.precision);
        }
        public static BigDecimal operator -(BigDecimal a, BigDecimal b)
        {
            return a + (-b);
        }
        public static BigDecimal operator *(BigDecimal a, BigDecimal b)
        {
            int highestprecision = a.precision >= b.precision ? a.precision : b.precision;
            return new BigDecimal(a.Mantissa * b.Mantissa, a.Power + b.Power, highestprecision);
        }
        public static BigDecimal operator /(BigDecimal a, BigDecimal b)
        {
            BigInteger remainder = a.Mantissa % b.Mantissa;
            BigInteger newMantissa = BigInteger.Divide(a.Mantissa, b.Mantissa);
            int newpower = 0;
            int digits = newMantissa.ToString().Length;
            int highestprecision = a.precision >= b.precision ? a.precision : b.precision;
            while (!(remainder == 0) && digits <= highestprecision)
            {
                remainder *= 10;
                newMantissa = newMantissa * 10 + remainder / b.Mantissa;
                newpower--;
                digits++;
                remainder %= b.Mantissa;
            }
            return new BigDecimal(newMantissa, a.Power - b.Power + newpower, highestprecision);
        }
        
        public  string toString()
        {
            return $"{Mantissa} * 10^{Power}";
        }
        public static int abs(int x)
        {
            return x < 0 ? -x : x;
        }
        public static implicit operator BigDecimal(int x)
        {
            return new BigDecimal(x, 0, 10);
        }
        public static BigDecimal Sqrt(BigDecimal a)
        {
            BigDecimal x = a/2;
            for (int i = 0; i < 100; i++)
            {
                x = (x + a / x) / 2;
            }
            return x;
        }
        public static BigDecimal Parse(string x)
        {
            BigInteger mantissa = 0;
            int power = 0; 
            for (int i = 0, passedPeriod = 0; i < x.Length; i++)
            {
                if (x[i] != '-')
                {
                    if(x[i] == '.')
                    {
                        passedPeriod++;
                    }
                    else
                    {
                        mantissa = mantissa * 10 + x[i] - '0';
                        if (passedPeriod != 0)
                        {
                            power--;
                        }
                    }
                }
            }
            return new BigDecimal(x[0] == '-' ? -mantissa : mantissa, power, x.Length-1); 
        }
        public static BigDecimal Pow(BigDecimal a, int b)
        {
            return new BigDecimal(BigInteger.Pow(a.Mantissa, b), a.Power * b, a.precision);
        }

    }

}
