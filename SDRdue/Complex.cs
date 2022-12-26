using System;

namespace PasiveRadar
{

    public struct Complex
    {
        public float Rea;
        public float Imag;

        public Complex(float real, float imaginary)
        {
            this.Rea = real;
            this.Imag = imaginary;
        }

        public float Re
        {
            get
            {
                return (Rea);
            }
            set
            {
                Rea = value;
            }
        }

        public float Im
        {
            get
            {
                return (Imag);
            }
            set
            {
                Imag = value;
            }
        }

        public override string ToString()
        {
            return (String.Format("({0}, {1}i)", Rea, Imag));
        }

        public static bool operator ==(Complex c1, Complex c2)
        {
            if ((c1.Rea == c2.Rea) &&
            (c1.Imag == c2.Imag))
                return (true);
            else
                return (false);
        }

        public static bool operator !=(Complex c1, Complex c2)
        {
            return (!(c1 == c2));
        }

        public override bool Equals(object o2)
        {
            Complex c2 = (Complex)o2;

            return (this == c2);
        }

        public override int GetHashCode()
        {
            return (Rea.GetHashCode() ^ Imag.GetHashCode());
        }

        public static Complex operator +(Complex c1, Complex c2)
        {
            return (new Complex(c1.Rea + c2.Rea, c1.Imag + c2.Imag));
        }

        public static Complex operator -(Complex c1, Complex c2)
        {
            return (new Complex(c1.Rea - c2.Rea, c1.Imag - c2.Imag));
        }

        // product of complex and float
        public static Complex operator *(Complex c1, float f)
        {
            return (new Complex(c1.Rea * f, c1.Imag * f));
        }

        // product of two complex numbers
        public static Complex operator *(Complex c1, Complex c2)
        {
            return (new Complex(c1.Rea * c2.Rea - c1.Imag * c2.Imag,
            c1.Rea * c2.Imag + c2.Rea * c1.Imag));
        }

        // quotient of two complex numbers
        public static Complex operator /(Complex c1, Complex c2)
        {
            if ((c2.Rea == 0.0f) &&
            (c2.Imag == 0.0f))
                throw new DivideByZeroException("Can't divide by zero Complex number");

            float newReal =
            (c1.Rea * c2.Rea + c1.Imag * c2.Imag) /
            (c2.Rea * c2.Rea + c2.Imag * c2.Imag);
            float newImaginary =
            (c2.Rea * c1.Imag - c1.Rea * c2.Imag) /
            (c2.Rea * c2.Rea + c2.Imag * c2.Imag);

            return (new Complex(newReal, newImaginary));
        }

        // non-operator versions for other languages

        // public static Complex Zero()
        // {
        //  Real(0);
        //   Imaginary(0);
        // }



        public double Module()
        {
            return (Math.Sqrt(Rea * Rea + Imag * Imag));
        }

    }

}
